using System.Text.Json;
using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.SceneManagement;

namespace Enigma_Framework.Core.ECS.ComponentLoaders;

[ComponentLoader("CircleCollider")]
public class CircleColliderLoader : IComponentLoader
{
    public void Load(Entity entity, JsonElement json)
    {
        var data = json.Deserialize<CircleColliderData>();

        if (data != null)
        {
             CircleCollider collider = (CircleCollider)new ColliderBuilder.Builder<CircleCollider>()
                .WithPhysicsMaterial(data.Material)
                .WithTriggerState(data.IsTrigger)
                .WithShapeArea(data.Area)
                .Build();
            entity.Components.Add(collider);
        }
    }
}