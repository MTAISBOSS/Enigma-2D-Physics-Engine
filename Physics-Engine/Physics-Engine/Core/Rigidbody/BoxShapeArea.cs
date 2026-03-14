namespace Physics_Engine.Core.Rigidbody
{
    public class BoxShapeArea : ShapeArea
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public override float Calculate() => Width * Height;
    }
}