using System;
using System.Collections.Generic;
using System.Linq;

namespace Physics_Engine.Core.Entity_Component_System;

public class ComponentContainer
{
    private readonly Entity _entity;
    private readonly Dictionary<Type, List<Component>> _components = new();

    public ComponentContainer(Entity entity)
    {
        _entity = entity;
    }

    public void Add<T>(T component) where T : Component
    {
        var type = typeof(T);
        if (!_components.ContainsKey(type))
            _components[type] = new List<Component>();

        if (!component.IsAbleToDuplicate() && _components[type].Any(c => c.GetType() == type))
            throw new InvalidOperationException($"Component {type.Name} cannot be duplicated.");

        if (component is Component baseComp)
        {
            baseComp.Entity = _entity;
            baseComp.Start();
        }

        _components[type].Add(component);
    }

    public bool TryGet<T>(out T component) where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var list) && list.FirstOrDefault() is T match)
        {
            component = match;
            return true;
        }

        component = default!;
        return false;
    }

    public T Get<T>() where T : Component
    {
        foreach (var componentList in _components.Values)
        {
            foreach (var component in componentList)
            {
                if (component is T typedComponent)
                {
                    return typedComponent;
                }
            }
        }

        return null;
    }

    public IEnumerable<T> GetAll<T>() where T : Component =>
        _components.TryGetValue(typeof(T), out var list)
            ? list.OfType<T>()
            : Enumerable.Empty<T>();

    public void Remove<T>() where T : Component
    {
        _components.Remove(typeof(T));
    }

    public void Update()
    {
        foreach (var component in _components)
        {
            foreach (var component1 in component.Value)
            {
                component1.Update();
            }
        }
    }
}