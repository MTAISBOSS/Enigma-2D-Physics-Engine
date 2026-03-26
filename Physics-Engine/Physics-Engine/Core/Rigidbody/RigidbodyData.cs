using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class RigidbodyData
    {
        public Vector2 LinearVelocity { get; set; }
        public float AngularVelocity { get; set; }
        public float Mass { get; set; } = 1;
        public float InverseMass => IsStatic ? 0f : 1f / Mass;
        public float Inertia { get; set; }
        public float InverseInertia => IsStatic ? 0f : 1f / Inertia;
        public bool IsStatic { get; set; }
        public bool HasGravity { get; set; }
    }
}