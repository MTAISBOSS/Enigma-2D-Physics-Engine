using Enigma_Framework.Core.ECS;

namespace Enigma_Framework.Core.SceneManagement;

public class SceneData
{
    public string Name { get; set; }
    public List<EntityData> Entities { get; set; } = new();
}