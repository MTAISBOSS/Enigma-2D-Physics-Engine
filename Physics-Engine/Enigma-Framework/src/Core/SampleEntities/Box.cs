using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;
using Physics_Engine.Graphics.Shapes;

namespace Enigma_Framework.Core.SampleEntities;

public class Box : Entity
{
    public Box()
    {
        Initialize();
    }

    public Box(string name = "", string tag = "") : base(name, tag)
    {
        Initialize();
    }

    private void Initialize()
    {
        Transform.WorldScale *= 4;
        var boxRenderer = new Rectangle
        {
            Filled = true,
            RenderOrder = 1,
            Entity = this
        };
        var rigidbody2D = new Rigidbody2D.Builder()
            .WithOwner(this)
            .WithMass(0.01f)
            .Build();
        var collider = new PolygonCollider
        {
            Entity = this,
            Material = new PhysicMaterial(1, 1, 1)
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(boxRenderer);
    }
}