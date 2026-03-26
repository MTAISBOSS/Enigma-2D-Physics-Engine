using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Entity_Component_System;
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
        var rigidbody2D = new RigidbodyBuilder.Builder<Rigidbody2D>()
            .WithOwner(this)
            .WithMass(0.01f)
            .Build();
        var collider = new PolygonCollider
        {
            Entity = this,
            Material = new PhysicMaterial(0,1,1,1)
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(boxRenderer);
    }
}