using System.Text.Json;
using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Utilities;

namespace Enigma_Framework.Core.SceneManagement;

public static class SceneSerializer
{
    public static void Save(Scene scene, string path)
    {
        var sceneData = new SceneData
        {
            Name = scene.Name
        };

        foreach (var entity in scene.Entities.GetEntities)
        {
            var entityData = new EntityData
            {
                id = entity.Id,
                Name = entity.Name
            };

            var transform = entity.Transform;

            entityData.Components["Transform"] = JsonHelper.ToJsonElement(new
            {
                Position = transform.LocalPosition,
                Scale = transform.LocalScale,
                Rotation = transform.LocalRotation,
                Parent = transform.Parent?.Entity?.Id
            });

            CheckOtherComponents(entity.Components, entityData);

            sceneData.Entities.Add(entityData);
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(sceneData, options);

        File.WriteAllText(path, json);
    }

    private static void CheckOtherComponents(
        ComponentContainer components,
        EntityData entityData)
    {
        if (components.TryGet(out PolygonCollider polygonCollider))
        {
            entityData.Components["PolygonCollider"] = JsonHelper.ToJsonElement(new
            {
                IsTrigger = polygonCollider.IsTrigger,
                PhysicMaterial = polygonCollider.Material,
                BoxArea = polygonCollider.BoxArea
            });
        }

        if (components.TryGet(out CircleCollider circleCollider))
        {
            entityData.Components["CircleCollider"] = JsonHelper.ToJsonElement(new
            {
                IsTrigger = circleCollider.IsTrigger,
                PhysicMaterial = circleCollider.Material,
                CircleArea = circleCollider.CircleArea
            });
        }

        if (components.TryGet(out Rigidbody2D rigidbody))
        {
            entityData.Components["Rigidbody2D"] = JsonHelper.ToJsonElement(new
            {
                LinearVelocityX = rigidbody.Body.LinearVelocity.x,
                LinearVelocityY = rigidbody.Body.LinearVelocity.y,
                AngularVelocity = rigidbody.Body.AngularVelocity,
                Mass = rigidbody.Body.Mass,
                InverseMass = rigidbody.Body.InverseMass,
                Inertia = rigidbody.Body.Inertia,
                InverseInertia = rigidbody.Body.InverseInertia,
                IsStatic = rigidbody.Body.IsStatic,
                HasGravity = rigidbody.Body.HasGravity
            });
        }
    }
}