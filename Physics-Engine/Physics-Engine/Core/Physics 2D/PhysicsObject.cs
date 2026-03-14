#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Component = Physics_Engine.Core.Physics_Engine.Core.Component;

namespace Physics_Engine.Core.Physics_2D;

public class PhysicsObject
{
    private readonly Dictionary<Type, Component> _components = new();
    public string Name { get; }
    public string Tag { get; }

    public PhysicsObject(string name = "",string tag ="")
    {
        Tag = tag;
        Name = name;
    }

    public T AddComponent<T>(T component) where T : class, Component
    {
        var type = typeof(T);

        if (_components.ContainsKey(type))
            throw new InvalidOperationException($"{type.Name} already exists on {Name}.");

        if (component is ComponentBase baseComp)
            baseComp.Owner = this;

        _components[type] = component;
        return component;
    }

    public bool TryGetComponent<T>(out T? component) where T : class, Component
    {
        if (_components.TryGetValue(typeof(T), out var c))
        {
            component = (T)c;
            return true;
        }

        component = null;
        return false;
    }

    public T? GetComponent<T>() where T : class, Component
    {
        if (TryGetComponent(out T? comp))
            return comp;

        throw new InvalidOperationException($"{typeof(T).Name} not found on {Name}");
    }

    public bool RemoveComponent<T>() where T : class, Component =>
        _components.Remove(typeof(T));
}