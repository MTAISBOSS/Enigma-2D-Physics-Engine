using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Graphics.Shapes;

namespace Physics_Engine.Core.Sample_Physic_Objects;

public class Box : Entity
{

    public Box() : base()
    {
        Initialize();
    }
    public Box(string name = "", string tag = "") : base(name, tag)
    {
        Initialize();
    }

    private void Initialize()
    {
        Transform.Scale *= 4;
        var boxRenderer = new Rectangle()
        {
            Filled = true,
            RenderOrder = 1,
            Entity = this,
        };
        var rigidbody2D = new RigidbodyBuilder.Builder<BoxRigidbody2D>()
            .WithArea(new BoxArea(Transform.Scale.x,Transform.Scale.y))
            .WithOwner(this)
            .WithDensity(0.5f)
            .WithRestitution(0)
            .Build();
        var collider = new PolygonCollider
        {
            Vertices = rigidbody2D.GetTransformedVertices(),
            Entity = this
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(boxRenderer);
    }
}