namespace Physics_Engine.Core.Rigidbody
{
    public class CircleShapeArea : ShapeArea
    {
        public float Radius { get; set; }
        public override float Calculate() => Radius * Radius * (float)System.Math.PI;
    }
}