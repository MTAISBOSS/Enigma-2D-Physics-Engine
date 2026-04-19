using System.Text.Json;

namespace Enigma_Framework.Core.ECS;

public interface IComponentLoader
{
    void Load(Entity entity, JsonElement json);
}