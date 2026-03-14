using Physics_Engine.Math;

namespace Physics_Engine.Core;

public readonly struct Transform
{
    public readonly float PositionX;
    public readonly float PositionY;
    public readonly float Sin;
    public readonly float Cos;
    public static readonly Transform Zero = new Transform(0f,0f,0f);

    public Transform(Vector2 position, float angle)
    {
        PositionX = position.x;
        PositionY = position.y;
        Sin = (float)System.Math.Sin(angle);
        Cos = (float)System.Math.Cos(angle);
    }
    public Transform(float x, float y, float angle)
    {
        PositionX = x;
        PositionY = y;
        Sin = (float)System.Math.Sin(angle);
        Cos = (float)System.Math.Cos(angle);
    }
}