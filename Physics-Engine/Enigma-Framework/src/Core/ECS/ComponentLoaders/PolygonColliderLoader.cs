using System.Text.Json;
using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.SceneManagement;

namespace Enigma_Framework.Core.ECS.ComponentLoaders;

[ComponentLoader("PolygonCollider")]
public class PolygonColliderLoader : IComponentLoader
{
    public void Load(Entity entity, JsonElement json)
    {
        var data = json.Deserialize<PolygonColliderData>();

        if (data != null)
        {
            PolygonCollider collider = (PolygonCollider)new ColliderBuilder.Builder<PolygonCollider>()
                .WithPhysicsMaterial(data.Material)
                .WithTriggerState(data.IsTrigger)
                .WithShapeArea(data.Area)
                .Build();
            entity.Components.Add(collider);
        }
    }
}