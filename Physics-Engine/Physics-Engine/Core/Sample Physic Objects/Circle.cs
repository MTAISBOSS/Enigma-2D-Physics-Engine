using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Entity_Component_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;

namespace Physics_Engine.Core.Sample_Physic_Objects;

public class Circle : Entity
{
    public Circle() : base()
    {
        Initialize();
    }
    public Circle(string name = "", string tag = "") : base(name, tag)
    {
        Initialize();
    }
    private void Initialize()
    {
        Transform.Scale *= 2;
        var renderer = new Graphics.Shapes.Circle()
        {
            Filled = true,
            Radius = Transform.Scale.x,
            RenderOrder = 1,
            Entity = this,
        };
        var rigidbody2D = new RigidbodyBuilder.Builder<CircleRigidbody2D>()
            .WithArea(new CircleArea()
            {
                Radius = Transform.Scale.x
            })
            .WithOwner(this)
            .Build();
        var collider = new CircleCollider()
        {
            Radius = Transform.Scale.x,
            Entity = this
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(renderer);
    }

}