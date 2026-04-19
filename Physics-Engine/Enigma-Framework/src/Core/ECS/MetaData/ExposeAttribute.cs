namespace Enigma_Framework.Core.ECS.MetaData;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ExposeAttribute : Attribute
{
    public string Name { get; set; }
    public float Min { get; set; } = float.NegativeInfinity;
    public float Max    { get; set; } = float.PositiveInfinity;
    public bool ReadOnly { get; set; }
    public string Tooltip { get; set; }

    public ExposeAttribute(string name = null)
    {
        Name = name;
    }
    
}