using System;
using System.Collections.Generic;
using System.Linq;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Physics_2D;

namespace Physics_Engine.Core.Component_System;

public class ComponentContainer
{
    private readonly PhysicsObject _owner;
    private readonly Dictionary<Type, List<IComponent>> _components = new();

    public ComponentContainer(PhysicsObject owner)
    {
        _owner = owner;
    }

    public void Add<T>(T component) where T : IComponent
    {
        var type = typeof(T);
        if (!_components.ContainsKey(type))
            _components[type] = new List<IComponent>();

        if (!component.IsAbleToDuplicate() && _components[type].Any(c => c.GetType() == type))
            throw new InvalidOperationException($"Component {type.Name} cannot be duplicated.");

        if (component is ComponentBase baseComp)
            baseComp.Owner = _owner;

        _components[type].Add(component);
    }

    public bool TryGet<T>(out T component) where T : IComponent
    {
        if (_components.TryGetValue(typeof(T), out var list) && list.FirstOrDefault() is T match)
        {
            component = match;
            return true;
        }

        component = default!;
        return false;
    }

    public T Get<T>() where T : class, IComponent
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



    public IEnumerable<T> GetAll<T>() where T : IComponent =>
        _components.TryGetValue(typeof(T), out var list)
            ? list.OfType<T>()
            : Enumerable.Empty<T>();

    public void Remove<T>() where T : IComponent
    {
        _components.Remove(typeof(T));
    }
}