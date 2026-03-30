using Physics_Engine.Math;

namespace Physics_Engine.Core.Raycast;

public struct Ray2D
{
    public Vector2 Origin;
    public Vector2 Direction;
    public float Lenght;

    public Ray2D(Vector2 origin, Vector2 direction, float lenght)
    {
        Origin = origin;
        Direction = direction;
        Lenght = lenght;
    }
}