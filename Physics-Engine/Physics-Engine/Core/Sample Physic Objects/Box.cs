using OpenTK.Graphics;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Graphics;
using Physics_Engine.Graphics.Shapes;

namespace Physics_Engine.Core.Sample_Physic_Objects;

public class Box : PhysicsObject
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
        var boxRenderer = new Rectangle()
        {
            Filled = true,
            Layer = 1,
            Owner = this,
        };
        var rigidbody2D = new RigidbodyBuilder.Builder<BoxRigidbody2D>()
            .WithArea(new BoxArea(1,1))
            .WithOwner(this)
            .Build();
        var collider = new PolygonCollider
        {
            Vertices = rigidbody2D.GetTransformedVertices(),
            Owner = this
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(boxRenderer);
    }
}