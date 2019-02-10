using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using Terraria;

// Author: 0x0ade - absolute wizard!
namespace Terraria3D
{
	internal static class XNAHacks
	{
		public static bool Applied { get; private set; } = false;

		// Check if we're actually running on XNA by checking the Game assembly name.
		// FNA doesn't care about graphics profiles.
		static readonly bool IsXNA = typeof(Game).Assembly.FullName.Contains("Microsoft.Xna.Framework");
		

		public static void Apply()
		{
			// Apply all XNA fixes before loading any resource from this mod.
			// XNAFixes.Apply() only runs once internally, and it needs to run earlier than Mod.Load()
			if (Main.dedServ || 
				Main.graphics.GraphicsProfile == GraphicsProfile.HiDef ||
				!Main.graphics.GraphicsDevice.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
				return;
			ApplySM3Fix();
			Applied = true;
		}

		private static void ApplySM3Fix()
		{
			// Only apply the fix if we need to apply it.
			const string AppliedName = "Terraria3D.XNAHacks.Applied";
			if (!IsXNA || Main.dedServ || "true".Equals(AppDomain.CurrentDomain.GetData(AppliedName)))
				return;
			AppDomain.CurrentDomain.SetData(AppliedName, "true");

			GraphicsDeviceManager gdm = Main.graphics;
			GraphicsDevice gd = gdm.GraphicsDevice;

			// Don't ever apply the fix if HiDef is already being used or not supported.
			if (gdm.GraphicsProfile == GraphicsProfile.HiDef || !gd.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
				return;

			/* Perform a one-time SM2 -> SM3 upgrade at runtime.
             * Note that we can't undo it. Good luck!
             * 
             * The following code is really unsafe.
             * We're rebuilding the StateTrackerDevice, which is seems to
             * be responsible for building events and keeping track of their
             * state.
             * 
             * It's the preferred method over changing the graphics profile,
             * as latter not only unloads all effects, but all graphics resources,
             * including textures.
             * Unfortunately, XNA doesn't cache the texture data.
             * Luckily, it caches effect data, which can be reloaded via dm.Reset()
             * 
             * Let's pray to redigit that there are no severe memleaks nor crashes.
             * -ade
             */

			/*
             * Mods are loaded on a separate thread, while graphics-critical
             * code must run on the main thread. This can be achieved by
             * temporarily hooking any method that runs on the main thread and
             * waiting for it to run.
             * Update and Render run at least once every frame.
             */
			bool applied = false;
			On.Terraria.Main.hook_Update hook = (origUpdate, self, gameTime) =>
			{
				origUpdate(self, gameTime);
				if (applied)
					return;

				// Obtain a reference to the graphics resource manager and some members.
				object pResourceManager = typeof(GraphicsDevice).GetField("pResourceManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gd);
				Type t_DeviceResourceManager = pResourceManager.GetType();

				// Make sure to end the current pass.
				FieldInfo f_activePass = typeof(GraphicsDevice).GetField("activePass", BindingFlags.NonPublic | BindingFlags.Instance);
				object activePass = f_activePass.GetValue(gd);
				if (activePass != null)
				{
					MethodInfo m_EndPass = typeof(EffectPass).GetMethod("EndPass", BindingFlags.NonPublic | BindingFlags.Instance);
					m_EndPass.Invoke(activePass, new object[0]);
					f_activePass.SetValue(gd, null);
				}

				// Unload all relevant graphics resources before trashing the state tracker.
				// This doesn't free all effects while also freeing too many unrelated resources.
				MethodInfo m_ReleaseAllDefaultPoolResources = t_DeviceResourceManager.GetMethod("ReleaseAllDefaultPoolResources");
				m_ReleaseAllDefaultPoolResources.Invoke(pResourceManager, new object[0]);

				// Replace the state tracker.
				UpdateStateTracker(gd);

				// Reload all relevant graphics resources.
				gd.Reset();

				// Obtain a reference to the resource data dictionary.
				object pResourceData = t_DeviceResourceManager.GetField("pResourceData", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(pResourceManager);
				MethodInfo m_set_Item = pResourceData.GetType().GetMethod("set_Item");

				// Update _all_ effects, preventing a crash on game exit.
				// This convoluted mess is necessary because the original ResourceData type is a non-public struct.
				List<DictionaryEntry> entries = new List<DictionaryEntry>();
				foreach (DictionaryEntry entry in (IDictionary)pResourceData)
					entries.Add(entry);
				object[] args_set_Item = new object[2];
				for (int i = 0; i < entries.Count; i++)
				{
					DictionaryEntry entry = entries[i];
					UpdateEffectEntry(ref entry);
					args_set_Item[0] = entry.Key;
					args_set_Item[1] = entry.Value;
					m_set_Item.Invoke(pResourceData, args_set_Item);
				}

				applied = true;
			};

			// Add the Update hook, wait for it to run and remove it.
			On.Terraria.Main.Update += hook;
			while (!applied)
				Thread.Sleep(0);
			On.Terraria.Main.Update -= hook;
		}

		// The following helpers transcend the boundaries of C#.

		#region HelperStateTrackerUpdater

		static readonly Action<GraphicsDevice> UpdateStateTracker = GenerateUpdateStateTracker();
		static Action<GraphicsDevice> GenerateUpdateStateTracker()
		{
			if (!IsXNA)
				return null;

			MethodInfo m_StateTrackerDeviceCtor = typeof(GraphicsDevice).Module.GetMethods((BindingFlags)(-1)).FirstOrDefault(m => m.Name == "Microsoft.Xna.Framework.Graphics.StateTrackerDevice.{ctor}");
			FieldInfo f_pStateTracker = typeof(GraphicsDevice).GetField("pStateTracker", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo f_pComPtr = typeof(GraphicsDevice).GetField("pComPtr", BindingFlags.NonPublic | BindingFlags.Instance);

			MethodInfo m_InvokeStateTrackerDtor = typeof(XNAHacks).GetMethod("InvokeStateTrackerDtor", BindingFlags.NonPublic | BindingFlags.Static);

			// Params: gd
			DynamicMethod dm = new DynamicMethod("XNAHacks.UpdateStateTracker", typeof(void), new Type[] { typeof(GraphicsDevice) }, typeof(GraphicsDevice), true);
			ILGenerator il = dm.GetILGenerator();

			// Destruct the existing state tracker.
			{
				// Load gd.pStateTracker
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, f_pStateTracker);

				il.Emit(OpCodes.Dup); // Duplicate pStateTracker, as it's also an argument.
									  // XNA only runs on x86. Let's perform 32-bit maths on the pointer.
				il.Emit(OpCodes.Ldind_I4); // Dereference the vtable ptr.
				il.Emit(OpCodes.Ldc_I4_8); // Offset to the destructor function pointer. Obtained by luck.
				il.Emit(OpCodes.Add);
				il.Emit(OpCodes.Ldind_I4); // Dereference the destructor function ptr.
				il.Emit(OpCodes.Call, m_InvokeStateTrackerDtor);
			}

			// Load the new state tracker.
			{
				// Put gd on the bottom of the stack for Stfld later.
				il.Emit(OpCodes.Ldarg_0);

				// Construct the new state tracker.
				{
					// Load gd.pStateTracker
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Ldfld, f_pStateTracker);

					// Load gd.pComPtr
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldfld, f_pComPtr);

					// Load uint 0x300 as the vertex and pixel shader versions.
					// This is also what HiDef passes.
					// We can use Ldc_I4 without Conv_U4
					il.Emit(OpCodes.Ldc_I4, 0x300U);
					il.Emit(OpCodes.Ldc_I4, 0x300U);

					// Call that horribly named native state tracker ctor.
					il.Emit(OpCodes.Call, m_StateTrackerDeviceCtor);
				}

				// Store the result into the previously pushed graphics device.
				il.Emit(OpCodes.Stfld, f_pStateTracker);
			}

			// Return from the method.
			il.Emit(OpCodes.Ret);

			return (Action<GraphicsDevice>)dm.CreateDelegate(typeof(Action<GraphicsDevice>));
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		delegate uint StateTrackerDtor(IntPtr pStateTracker);
		static void InvokeStateTrackerDtor(IntPtr pStateTracker, IntPtr ftnPtr)
		{
			StateTrackerDtor dtor = (StateTrackerDtor)Marshal.GetDelegateForFunctionPointer(ftnPtr, typeof(StateTrackerDtor));
			dtor(pStateTracker);
		}

		#endregion

		#region HelperEffectUpdater

		delegate void DelegateUpdateEffectEntry(ref DictionaryEntry entry);
		static readonly DelegateUpdateEffectEntry UpdateEffectEntry = GenerateUpdateEffectEntry();
		static DelegateUpdateEffectEntry GenerateUpdateEffectEntry()
		{
			if (!IsXNA)
				return null;

			PropertyInfo p_Value = typeof(DictionaryEntry).GetProperty("Value");
			MethodInfo m_get_Value = p_Value.GetMethod;
			MethodInfo m_set_Value = p_Value.SetMethod;

			Type t_ResourceData = typeof(GraphicsDevice).Assembly.GetType("Microsoft.Xna.Framework.Graphics.ResourceData");
			MethodInfo m_UpdateEffectUpdater = typeof(XNAHacks).GetMethod("UpdateEffectData", BindingFlags.NonPublic | BindingFlags.Static);

			// Params: entry
			DynamicMethod dm = new DynamicMethod("XNAHacks.UpdateEffectEntry", typeof(void), new Type[] { typeof(DictionaryEntry).MakeByRefType() }, typeof(GraphicsDevice), true);
			ILGenerator il = dm.GetILGenerator();

			{
				// Put entry on the bottom of the stack for set_Value later.
				il.Emit(OpCodes.Ldarg_0);

				{
					// Get the value, then call UpdateEffectUpdater which returns an updated value.
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Call, m_get_Value);

					il.Emit(OpCodes.Unbox_Any, t_ResourceData);

					il.Emit(OpCodes.Call, m_UpdateEffectUpdater);

					il.Emit(OpCodes.Box, t_ResourceData);
				}

				// Store the result into the previously pushed entry.
				il.Emit(OpCodes.Call, m_set_Value);
			}

			// Return from the method.
			il.Emit(OpCodes.Ret);

			return (DelegateUpdateEffectEntry)dm.CreateDelegate(typeof(DelegateUpdateEffectEntry));
		}

		static readonly FieldInfo f_Effect_isDisposed = typeof(Effect).GetField("isDisposed", BindingFlags.NonPublic | BindingFlags.Instance);
		static readonly FieldInfo f_Effect_pComPtr = typeof(Effect).GetField("pComPtr", BindingFlags.NonPublic | BindingFlags.Instance);
		static readonly MethodInfo m_Effect_ReleaseNativeObject = typeof(Effect).GetMethod("ReleaseNativeObject", BindingFlags.NonPublic | BindingFlags.Instance);
		static readonly MethodInfo m_Effect_RecreateAndPopulateObject = typeof(Effect).GetMethod("RecreateAndPopulateObject", BindingFlags.NonPublic | BindingFlags.Instance);
		static ResourceData UpdateEffectData(ResourceData data)
		{
			if (data.isDisposed)
				return data;

			Effect effect = data.ManagedObject.Target as Effect;
			if (effect == null)
				return data;

			if (!((bool)f_Effect_isDisposed.GetValue(effect)))
			{
				// ReleaseNativeObject kills everything immediately.
				// m_Effect_ReleaseNativeObject.Invoke(effect, new object[] { false });

				// Instead, let's just embrace the memory leak.
				f_Effect_pComPtr.SetValue(effect, null);

				// Recreate the effect _again_, this time to obtain a new valid pComPtr.
				m_Effect_RecreateAndPopulateObject.Invoke(effect, new object[0]);
				data.pComPtr = UnboxPtr(f_Effect_pComPtr.GetValue(effect));
			}

			return data;
		}

		static readonly Func<object, IntPtr> UnboxPtr = GenerateUnboxPtr();
		static Func<object, IntPtr> GenerateUnboxPtr()
		{
			if (!IsXNA)
				return null;

			MethodInfo m_Unbox = typeof(Pointer).GetMethod("Unbox");

			// Params: ptrbox
			DynamicMethod dm = new DynamicMethod("XNAHacks.UnboxPtr", typeof(IntPtr), new Type[] { typeof(object) }, typeof(GraphicsDevice), true);
			ILGenerator il = dm.GetILGenerator();

			// Load object, unbox to void*, return as IntPtr, profit.
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Call, m_Unbox);
			il.Emit(OpCodes.Ret);

			return (Func<object, IntPtr>)dm.CreateDelegate(typeof(Func<object, IntPtr>));
		}

		// This struct matches XNA's internal ResourceData struct as close as possible.
#pragma warning disable CS0649
		struct ResourceData
		{
			public string ResourceName;
			public IntPtr pComPtr;
			public object ResourceTag;
			public WeakReference ManagedObject;
			public uint dwResourceManagementMode;
			public int CurrentRefCount;
			public ulong objectHandle;
			public bool isDisposed;
		}
#pragma warning restore CS0649

		#endregion

	}
}
