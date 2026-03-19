using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class PhysicsBody
    {
        public Vector2 LinearVelocity { get; set; }
        public float AngularVelocity { get; set; }
        public float Mass => ShapeArea.Calculate() * Density;
        public float InverseMass => IsStatic ? 0f : 1f / Mass;
        public float Density { get; set; } = 1;
        public float Bounciness { get; set; } = 1;
        public ShapeArea ShapeArea { get; set; }
        public bool IsStatic { get; set; }
        public bool HasGravity { get; set; }
        public float Restitution
        {
            get;
            set;
        } = 0.6f;
        public Vector2[] Vertices { get; set; }
        public Vector2[] TransformedVertices { get; set; }
        public bool IsTransformUpdateRequired = true;
        public int[] Indices { get; set; }
        public AABBCollision AABBCollision;
        public bool IsAABBCollisionUpdateRequired = true;
    }
}