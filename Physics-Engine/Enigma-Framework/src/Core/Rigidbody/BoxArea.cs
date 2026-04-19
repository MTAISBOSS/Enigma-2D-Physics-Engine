namespace Enigma_Framework.Core.Rigidbody;

public class BoxArea : ShapeArea
{
    public BoxArea(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public float Width { get; }
    public float Height { get; }

    public override float Calculate()
    {
        return Width * Height;
    }
}