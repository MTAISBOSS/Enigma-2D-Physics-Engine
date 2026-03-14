using Physics_Engine.Core.Physics_Engine.Core;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public abstract class Rigidbody2D : ComponentBase, IMovement, IRotation
    {
        public Body Body { get; private set; }

        public class Builder<T> where T : Rigidbody2D, new()
        {
            private ShapeArea _shapeArea;
            private Vector2 _position = Vector2.Zero;
            private Vector2 _linearVelocity = Vector2.Zero;
            private Vector2 _angularVelocity = Vector2.Zero;
            private float _rotation;
            private float _density = 1;
            private float _bounciness = 1;
            private float _restitution = 1;
            private bool _isStatic;

            public Builder<T> WithPosition(Vector2 position)
            {
                _position = position;
                return this;
            }

            public Builder<T> WithRotation(float rotation)
            {
                _rotation = rotation;
                return this;
            }

            public Builder<T> WithLinearVelocity(Vector2 velocity)
            {
                _linearVelocity = velocity;
                return this;
            }

            public Builder<T> WithAngularVelocity(Vector2 velocity)
            {
                _angularVelocity = velocity;
                return this;
            }

            public Builder<T> WithDensity(float density)
            {
                _density = density;
                return this;
            }

            public Builder<T> WithBounciness(float bounciness)
            {
                _bounciness = bounciness;
                return this;
            }

            public Builder<T> WithState(bool isStatic)
            {
                _isStatic = isStatic;
                return this;
            }

            public Builder<T> WithRestitution(float restitution)
            {
                _restitution = Mathematics.Clamp(restitution, 0, 1);
                return this;
            }

            public Builder<T> WithArea(ShapeArea shapeArea)
            {
                _shapeArea = shapeArea;
                return this;
            }

            public T Build()
            {
                T body = new T
                {
                    Body = new Body()
                    {
                        Density = _density,
                        Bounciness = _bounciness,
                        IsStatic = _isStatic,
                        Position = _position,
                        Rotation = _rotation,
                        LinearVelocity = _linearVelocity,
                        AngularVelocity = _angularVelocity,
                        Restitution = _restitution,
                        ShapeArea = _shapeArea
                    }
                };
                bool isAbleToCreateBody = body.TryCreate();
                return isAbleToCreateBody ? body : null;
            }
        }

        protected abstract bool TryCreate();

        public void Move(Vector2 amount)
        {
            Body.Position += amount;
            Body.IsTransformUpdateRequired = true;
        }

        public void MoveTowards(Vector2 position)
        {
            Body.Position = position;
            Body.IsTransformUpdateRequired = true;
        }

        public override bool IsAbleToBeCloned()
        {
            return false;
        }

        public void Rotate(float amount)
        {
            Body.Rotation += amount;
            Body.IsTransformUpdateRequired = true;
        }
    }
}