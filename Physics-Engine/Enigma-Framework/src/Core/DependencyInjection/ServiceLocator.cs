namespace Enigma_Framework.Core.DependencyInjection;

public class ServiceLocator
{
    private static ServiceLocator _instance;

    private readonly Dictionary<string, IService> services = new();

    private ServiceLocator()
    {
    }

    public static ServiceLocator Instance
    {
        get
        {
            _instance ??= new ServiceLocator();
            return _instance;
        }
    }

    public void Register<T>(T service) where T : IService
    {
        var key = typeof(T).Name;
        if (services.ContainsKey(key)) return;

        services.Add(key, service);
    }

    public T Get<T>() where T : IService
    {
        var key = typeof(T).Name;
        if (services.ContainsKey(key)) return (T)services[key];

        throw new ArgumentException($"There is no service with type {key}");
    }

    public void Unregister<T>(T service) where T : IService
    {
        var key = typeof(T).Name;
        if (!services.ContainsKey(key)) return;

        services.Remove(key);
    }
}