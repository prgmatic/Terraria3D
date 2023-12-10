using System.Linq;

namespace Terraria3D.Serialization;

public class SettingsData
{
	public bool Enabled { get; set; }
	public SceneData Scene { get; set; } = new SceneData();
	public LayerData[] LayerData { get; set; }

	public void Apply(Terraria3D mod)
	{
		Terraria3D.Enabled = Enabled;
		Scene.Apply(mod.Scene);
		ApplyLayers(mod.LayerManager.Layers);
	}

	public void Record(Terraria3D mod)
	{
		Enabled = Terraria3D.Enabled;
		Scene.Record(mod.Scene);
		RecordLayers(mod.LayerManager.Layers);
	}

	private void ApplyLayers(Layer3D[] layers)
	{
		foreach(var layer in layers)
			LayerData.Where(ld => ld.Name.Equals(layer.Name)).FirstOrDefault()?.Apply(layer);
	}
	private void RecordLayers(Layer3D[] layers)
	{
		LayerData = new LayerData[layers.Length];
		for (int i = 0; i < layers.Length; i++)
			LayerData[i] = new LayerData(layers[i]);
	}
}