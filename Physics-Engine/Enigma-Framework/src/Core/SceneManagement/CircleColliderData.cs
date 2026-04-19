using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;

namespace Enigma_Framework.Core.SceneManagement;

public class CircleColliderData : ComponentData
{
    public bool IsTrigger { get; set; }
    public PhysicMaterial Material { get; set; }
    public CircleArea Area { get; set; }
}