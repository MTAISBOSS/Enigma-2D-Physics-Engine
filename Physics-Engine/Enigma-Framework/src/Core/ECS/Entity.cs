using Enigma_Framework.Core.DependencyInjection;

namespace Enigma_Framework.Core.ECS;

public class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    private Enigma_Framework.Core.Transform.Transform transform;
    public Entity(string name = "", string tag = "")
    {
        Name = name;
        Tag = tag;
        IsActive = true;
        Components = new ComponentContainer(this);
        var physicsSystem = ServiceLocator.Instance.Get<EntityContainer>();
        physicsSystem.RegisterEntity(this);
    }

    public string Name { get; }
    public string Tag { get; }
    public bool IsActive { get; private set; }

    public Enigma_Framework.Core.Transform.Transform Transform
    {
        get
        {
            if (transform == null)
            {
                transform ??= new Enigma_Framework.Core.Transform.Transform();
                Components.Add(transform);
            }

            return transform;
        }
        set => transform = value;
    }

    public ComponentContainer Components { get; }

    ~Entity()
    {
        var physicsSystem = ServiceLocator.Instance.Get<EntityContainer>();
        physicsSystem.UnregisterEntity(this);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}