namespace Enigma_Framework.Core.ECS;

public class ComponentContainer
{
    private readonly Dictionary<Type, List<Component>> components = new();
    private readonly Entity? entity;

    public ComponentContainer(Entity? entity)
    {
        this.entity = entity;
    }

    public void Add<T>(T component) where T : Component
    {
        var type = typeof(T);
        if (!components.ContainsKey(type))
            components[type] = new List<Component>();

        if (!component.IsAbleToDuplicate() && components[type].Any(c => c.GetType() == type))
            throw new InvalidOperationException($"Component {type.Name} cannot be duplicated.");

        if (component is Component baseComp)
        {
            baseComp.Entity = entity;
            baseComp.Start();
        }

        components[type].Add(component);
    }

    public bool TryGet<T>(out T component) where T : Component
    {
        component = null;
        foreach (var componentList in components.Values)
        foreach (var comp in componentList)
        {
            if (comp is T typedComponent)
            {
                component = typedComponent;
                return true;
            }
        }
        
        return false;
    }

    public T Get<T>() where T : Component
    {
        foreach (var componentList in components.Values)
        foreach (var component in componentList)
            if (component is T typedComponent)
                return typedComponent;

        return null;
    }

    public IEnumerable<T> GetAll<T>() where T : Component
    {
        return components.TryGetValue(typeof(T), out var list)
            ? list.OfType<T>()
            : Enumerable.Empty<T>();
    }

    public void Remove<T>() where T : Component
    {
        components.Remove(typeof(T));
    }

    public void Update()
    {
        foreach (var component in components)
        foreach (var component1 in component.Value)
            component1.Update();
    }
}