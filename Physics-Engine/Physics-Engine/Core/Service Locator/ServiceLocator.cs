using System;
using System.Collections.Generic;

namespace Physics_Engine.Core.Service_Locator;

public class ServiceLocator
{
    private static ServiceLocator _instance;

    public static ServiceLocator Instance
    {
        get
        {
            _instance ??= new ServiceLocator();
            return _instance;
        }
    }

    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    private ServiceLocator() { }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            return;
        }

        _services.Add(key, service);
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            return (T)_services[key];
        }

        throw new ArgumentException($"There is no service with type {key}");
    }

    public void Unregister<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            return;
        }

        _services.Remove(key);
    }
}