using Enigma_Framework.Core.DependencyInjection;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Rigidbody;

public partial class Rigidbody2D
{
    public class Builder
    {
        private readonly Rigidbody2D instance = new();

        public Builder WithLinearVelocity(Vector2 velocity)
        {
            instance.Body.LinearVelocity = velocity;
            return this;
        }

        public Builder WithAngularVelocity(float velocity)
        {
            instance.Body.AngularVelocity = velocity;
            return this;
        }

        public Builder WithMass(float mass)
        {
            instance.Body.Mass = mass;
            return this;
        }


        public Builder WithState(bool isStatic)
        {
            instance.Body.IsStatic = isStatic;
            return this;
        }

        public Builder WithOwner(Entity? entity)
        {
            instance.Entity = entity;
            return this;
        }

        public Builder WithGravityState(bool hasGravity)
        {
            instance.Body.HasGravity = hasGravity;
            return this;
        }

        public Rigidbody2D Build()
        {
            var isAbleToCreateBody = instance.TryCreate();
            PhysicsContext.RegisterRigidbody(instance);
            return isAbleToCreateBody ? instance : null;
        }
    }
}