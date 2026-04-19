using System.Text.Json;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Math;
using RigidbodyData = Enigma_Framework.Core.SceneManagement.RigidbodyData;

namespace Enigma_Framework.Core.ECS.ComponentLoaders;

[ComponentLoader("Rigidbody2D")]
public class RigidbodyLoader : IComponentLoader
{
    public void Load(Entity entity, JsonElement json)
    {
        var data = json.Deserialize<RigidbodyData>();
        if (data != null)
        {
            Rigidbody2D rb = new Rigidbody2D.Builder()
                .WithMass(data.Mass)
                .WithState(data.IsStatic)
                .WithOwner(entity)
                .WithGravityState(data.HasGravity)
                .WithAngularVelocity(data.AngularVelocity)
                .WithLinearVelocity(data.LinearVelocity)
                .Build();
            entity.Components.Add(rb);
        }
    }
}