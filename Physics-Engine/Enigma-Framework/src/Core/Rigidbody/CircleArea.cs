namespace Enigma_Framework.Core.Rigidbody;

public class CircleArea : ShapeArea
{
    public CircleArea(float radius)
    {
        Radius = radius;
    }

    public float Radius { get; }

    public override float Calculate()
    {
        return Radius * Radius * (float)System.Math.PI;
    }
}