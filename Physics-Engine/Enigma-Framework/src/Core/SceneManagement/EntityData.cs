using System.Text.Json;

namespace Enigma_Framework.Core.SceneManagement;

public class EntityData
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public Dictionary<string,JsonElement> Components { get; set; } = new();
}