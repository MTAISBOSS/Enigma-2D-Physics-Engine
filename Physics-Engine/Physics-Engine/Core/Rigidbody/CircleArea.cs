namespace Physics_Engine.Core.Rigidbody
{
    public class CircleArea : ShapeArea
    {
        public float Radius { get; }

        public CircleArea(float radius)
        {
            Radius = radius;
        }
        public override float Calculate() => Radius * Radius * (float)System.Math.PI;
    }
}