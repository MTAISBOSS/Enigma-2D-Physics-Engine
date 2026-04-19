namespace Enigma_Framework.Core.ECS;

[AttributeUsage(AttributeTargets.Class)]
public class ComponentLoaderAttribute : Attribute
{
    public string Name { get; }

    public ComponentLoaderAttribute(string name)
    {
        Name = name;
    }
}