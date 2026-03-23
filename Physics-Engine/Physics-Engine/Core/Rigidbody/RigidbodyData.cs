using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class RigidbodyData
    {
        public Vector2 LinearVelocity { get; set; }
        public float AngularVelocity { get; set; }
        public float Mass => ShapeArea.Calculate() * Density * 0.01f;
        public float InverseMass => IsStatic ? 0f : 1f / Mass;
        public float Density { get; set; }
        public float Bounciness { get; set; }
        public float Inertia { get; set; }
        public float InverseInertia => IsStatic ? 0f : 1f / Inertia;
        public ShapeArea ShapeArea { get; set; }
        public bool IsStatic { get; set; }
        public bool HasGravity { get; set; }
        public float StaticFriction { get; set; } = 0.6f;
        public float DynamicFriction { get; set; } = 0.4f;
        public float Restitution { get; set; }
        public Vector2[] Vertices { get; set; }
        public Vector2[] TransformedVertices { get; set; }
        public bool IsTransformUpdateRequired = true;
        public int[] Indices { get; set; }
        public AABBCollision AABBCollision { get; set; }
        public bool IsAabbCollisionUpdateRequired = true;
    }
}