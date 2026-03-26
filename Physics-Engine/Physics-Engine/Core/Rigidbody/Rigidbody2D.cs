using System.Collections.Generic;
using System.Linq;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Entity_Component_System;
using Physics_Engine.Core.Interfaces;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Transform;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class Rigidbody2D : Component, IMovement, IRotation
    {
        private Vector2 force = Vector2.Zero;
        private Transform.Transform Transform => Entity.Transform;
        public RigidbodyData Body { get; } = new();

        public float Rotation
        {
            get => Transform.Rotation;
            set
            {
                Transform.Rotation = value;
                UpdateColliderCaches();
            }
        }

        public Vector2 Position
        {
            get => Transform.Position;
            set
            {
                Transform.Position = value;
                UpdateColliderCaches();
            }
        }

        public override void Start()
        {
            var collider = Entity.Components.Get<Collider>();
            if (collider == null)
            {
                return;
            }
            Body.Inertia = collider.CalculateRotationalInertia(Body.Mass);
        }

        public bool TryCreate()
        {
            return this.ValidateMaxDensity() && this.ValidateMinDensity();
        }

        public void MoveByAmount(Vector2 amount)
        {
            Transform.Translate(amount);
            UpdateColliderCaches();
        }

        public void MoveToExactPosition(Vector2 position)
        {
            Transform.Position = position;
            UpdateColliderCaches();
        }

        public void RotateToExactAngle(float angle)
        {
            Transform.Rotation = angle;
            UpdateColliderCaches();
        }

        public void RotateByAmount(float amount)
        {
            Transform.Rotate(amount);
            UpdateColliderCaches();
        }

        public void Simulate(float time)
        {
            if (Body.IsStatic) return;

            time /= PhysicsSetting.Iterations;

            Vector2 acceleration = force / Body.Mass;
            if (Body.HasGravity)
            {
                acceleration += PhysicsSetting.GravityDirection;
            }

            Body.LinearVelocity += acceleration * time;

            Position += Body.LinearVelocity * time;
            Rotation += Body.AngularVelocity * time;
            force = Vector2.Zero;

            UpdateColliderCaches();
        }

        public void AddForce(Vector2 force)
        {
            this.force = force;
        }

        public override bool IsAbleToDuplicate()
        {
            return false;
        }

        public AABBCollision GetAABB()
        {
            return Entity.Components.Get<Collider>().GetAABB();
        }

        private void UpdateColliderCaches()
        {
            var collider = Entity.Components.Get<Collider>();
            collider?.InvalidateCaches();
        }
    }
}