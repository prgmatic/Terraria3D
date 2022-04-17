namespace Terraria3D.Serialization;

public class SceneData
{
	public CameraData Camera { get; set; } = new CameraData();
	public bool AmbientOcclusionEnabled { get; set; }

	public void Record(Scene3D scene)
	{
		Camera.Record(scene);
		AmbientOcclusionEnabled = scene.AmbientOcclusion;
	}

	public void Apply(Scene3D scene)
	{
		Camera.Apply(scene);
		scene.AmbientOcclusion = AmbientOcclusionEnabled;
	}
}