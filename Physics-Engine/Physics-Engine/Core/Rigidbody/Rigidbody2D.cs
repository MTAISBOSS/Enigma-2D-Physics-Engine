using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Component_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Transform;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public abstract class Rigidbody2D : ComponentBase, IMovement, IRotation
    {
        private Vector2 _force = Vector2.Zero;
        private Transform2D Transform => Owner.Transform;
        public PhysicsBody Body { get; } = new();

        public float Rotation
        {
            get => Transform.Rotation;
            set
            {
                Transform.Rotation = value;
                Body.IsTransformUpdateRequired = true;
            }
        }
        public Vector2 Position
        {
            get => Transform.Position;
            set
            {
                Transform.Position = value;
                Body.IsTransformUpdateRequired = true;
            }
        }
        public abstract bool TryCreate();
        public abstract AABBCollision GetAABB();
        public void MoveByAmount(Vector2 amount)
        {
            Transform.Translate(amount);
            Body.IsTransformUpdateRequired = true;
            Body.IsAabbCollisionUpdateRequired = true;
        }

        public void MoveToExactPosition(Vector2 position)
        {
            Transform.Position = position;
            Body.IsTransformUpdateRequired = true;
            Body.IsAabbCollisionUpdateRequired = true;
        }

        public void RotateByAmount(float amount)
        {
            Transform.Rotate(amount);
            Body.IsTransformUpdateRequired = true;
            Body.IsAabbCollisionUpdateRequired = true;
        }

        public void Simulate(float time)
        {
            if (Body.IsStatic)
            {
                return;
            }

            time /= PhysicsSetting.Iterations;
            if (Body.HasGravity)
            {
                Body.LinearVelocity += PhysicsSetting.GravityDirection * time;
            }
            else
            {
                Body.LinearVelocity += _force / Body.Mass * time;
            }

            Position += Body.LinearVelocity * time;
            Rotation += Body.AngularVelocity * time;
            _force = Vector2.Zero;
            Body.IsTransformUpdateRequired = true;
            Body.IsAabbCollisionUpdateRequired = true;
        }

        public void AddForce(Vector2 force)
        {
            _force = force;
        }

        public override bool IsAbleToDuplicate()
        {
            return false;
        }
    }
}