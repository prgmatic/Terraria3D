using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;

namespace Terraria3D;

public partial class CameraDriver
{
	private static AxisBindings _forwardAxisInput = new AxisBindings("Up", "Down");
	private static AxisBindings _rightAxisInput = new AxisBindings("Right", "Left");
	private static AxisKeys _upAxisInput = new AxisKeys(Keys.E, Keys.Q);
	private static Keys _resetKey = Keys.F;

	abstract class Axis<T>
	{
		public T Positive { get; set; }
		public T Negative { get; set; }

		public Axis(T positive, T negative)
		{
			Positive = positive;
			Negative = negative;
		}
		public abstract float GetValue();
	}

	private class AxisBindings : Axis<string>
	{
		public AxisBindings(string positive, string negative) : base(positive, negative) { }

		public override float GetValue()
		{
			float result = 0;
			if (PlayerInput.Triggers.Current.KeyStatus[Negative]) result -= 1;
			if (PlayerInput.Triggers.Current.KeyStatus[Positive]) result += 1;
			return result;
		}
	}

	private class AxisKeys : Axis<Keys>
	{
		public AxisKeys(Keys positive, Keys negative) : base(positive, negative) { }

		public override float GetValue()
		{
			float result = 0;
			if (Main.keyState.IsKeyDown(Negative)) result -= 1;
			if (Main.keyState.IsKeyDown(Positive)) result += 1;
			return result;
		}
	}
}