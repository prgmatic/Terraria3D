namespace Terraria3D.Serialization
{
	public class LayerData
	{
		public string Name { get; set; }
		public bool Enabled { get; set; }
		public float Depth { get; set; }
		public float ZPos { get; set; }
		public float Noise { get; set; }
		public bool UseInnerPixel { get; set; }

		public LayerData() { }
		public LayerData(Layer3D layer)
		{
			Record(layer);
		}

		public void Apply(Layer3D layer)
		{
			layer.Enabled = Enabled;
			layer.Depth = Depth;
			layer.ZPos = ZPos;
			layer.NoiseAmount = Noise;
			layer.UseInnerPixel = UseInnerPixel;
		}

		public void Record(Layer3D layer)
		{
			Name = layer.Name;
			Enabled = layer.Enabled;
			Depth = layer.Depth;
			ZPos = layer.ZPos;
			Noise = layer.NoiseAmount;
			UseInnerPixel = layer.UseInnerPixel;
		}
	}
}
