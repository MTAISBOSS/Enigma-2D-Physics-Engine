using System.Text.Json;
using Enigma_Framework.Core.SceneManagement;

namespace Enigma_Framework.Core.ECS.ComponentLoaders;

[ComponentLoader("Transform")]
public class TransformLoader : IComponentLoader
{
    public void Load(Entity entity, JsonElement json)
    {
        var data = json.Deserialize<TransformData>();

        var transform = entity.Transform;

        if (data != null)
        {
            transform.LocalPosition = data.Position;
            transform.LocalScale = data.Scale;
            transform.LocalRotation = data.Rotation;
        }
    }
}