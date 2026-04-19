using Enigma_Framework.Math;

namespace Enigma_Framework.Core.SceneManagement;

public class TransformData : ComponentData
{
    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public float Rotation { get; set; }

    public Guid? Parent { get; set; }
}