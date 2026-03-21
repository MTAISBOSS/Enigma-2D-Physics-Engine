using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Service_Locator;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody;

public class RigidbodyBuilder
{
    public class Builder<T> where T : Rigidbody2D, new()
    {
        private readonly T _instance = new();

        public Builder<T> WithPosition(Vector2 position)
        {
            _instance.Position = position;
            return this;
        }

        public Builder<T> WithRotation(float rotation)
        {
            _instance.Rotation = rotation;
            return this;
        }

        public Builder<T> WithLinearVelocity(Vector2 velocity)
        {
            _instance.Body.LinearVelocity = velocity;
            return this;
        }

        public Builder<T> WithAngularVelocity(float velocity)
        {
            _instance.Body.AngularVelocity = velocity;
            return this;
        }

        public Builder<T> WithDensity(float density)
        {
            _instance.Body.Density = density;
            return this;
        }

        public Builder<T> WithBounciness(float bounciness)
        {
            _instance.Body.Bounciness = bounciness;
            return this;
        }

        public Builder<T> WithState(bool isStatic)
        {
            _instance.Body.IsStatic = isStatic;
            return this;
        }

        public Builder<T> WithRestitution(float restitution)
        {
            _instance.Body.Restitution = Mathematics.Clamp(restitution, 0, 1);
            return this;
        }

        public Builder<T> WithArea(ShapeArea shapeArea)
        {
            _instance.Body.ShapeArea = shapeArea;
            return this;
        }
        public Builder<T> WithOwner(PhysicsObject owner)
        {
            _instance.Owner = owner;
            return this;
        } 
        public Builder<T> WithGravityState(bool hasGravity)
        {
            _instance.Body.HasGravity = hasGravity;
            return this;
        }
        public T Build()
        {
            bool isAbleToCreateBody = _instance.TryCreate();
            ServiceLocator.Instance.Get<PhysicsWorld>().RegisterRigidbody(_instance);
            return isAbleToCreateBody ? _instance : null;
        }
    }
}