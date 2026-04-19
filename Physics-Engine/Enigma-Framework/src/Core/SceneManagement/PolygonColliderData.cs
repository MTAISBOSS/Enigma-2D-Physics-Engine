using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;

namespace Enigma_Framework.Core.SceneManagement;

public class PolygonColliderData : ComponentData
{
    public bool IsTrigger { get; set; }
    public PhysicMaterial Material { get; set; }
    public BoxArea Area { get; set; }
}