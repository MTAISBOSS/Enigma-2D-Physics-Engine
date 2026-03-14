using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class Body
    {
        public Vector2 LinearVelocity { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 AngularVelocity { get; set; }
        public float Rotation { get; set; }

        public float Mass => ShapeArea.Calculate() * Density;

        public float Density { get; set; }
        public float Bounciness { get; set; }
        public ShapeArea ShapeArea { get; set; }
        public bool IsStatic { get; set; }

        public float Restitution
        {
            get => Restitution;
            set { value = Mathematics.Clamp(value, 0, 1); }
        }

        public Vector2[] Vertices { get; set; }
        public Vector2[] TransformedVertices { get; set; }
        public bool IsTransformUpdateRequired = true;
        public int[] Indices { get; set; }
    }
}