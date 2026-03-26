namespace Physics_Engine.Core.Rigidbody
{
    public class BoxArea : ShapeArea
    {
        public float Width { get; }
        public float Height { get; }

        public BoxArea(float width,float height)
        {
            Width = width;
            Height = height;
        }
        public override float Calculate() => Width * Height;
    }
}