using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Rigidbody;

namespace Enigma_Framework.Core.SampleEntities;

public class Circle : Entity
{
    public Circle()
    {
        Initialize();
    }

    public Circle(string name = "", string tag = "") : base(name, tag)
    {
        Initialize();
    }

    private void Initialize()
    {
        Transform.WorldScale *= 2;
        var renderer = new Enigma_Framework.Graphics.Shapes.Circle
        {
            Filled = true,
            Radius = Transform.WorldScale.x,
            RenderOrder = 1,
            Entity = this
        };
        var rigidbody2D = new Rigidbody2D.Builder()
            .WithOwner(this)
            .Build();
        var collider = new CircleCollider
        {
            Entity = this
        };
        Components.Add(collider);
        Components.Add(rigidbody2D);
        Components.Add(renderer);
    }
}