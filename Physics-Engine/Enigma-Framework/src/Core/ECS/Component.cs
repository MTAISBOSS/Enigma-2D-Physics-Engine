namespace Enigma_Framework.Core.ECS;

public abstract class Component
{
    public Entity? Entity { get; internal set; }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual bool IsAbleToDuplicate()
    {
        return true;
    }
}