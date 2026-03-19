using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public class AABBCollision
{
    public readonly Vector2 Min;
    public readonly Vector2 Max;
    public AABBCollision(Vector2 min,Vector2 max)
    {
        Min = min;
        Max = max;
    }
    public AABBCollision(float minX,float minY,float maxX,float maxY)
    {
        Min.x = minX;
        Min.y = minY;
        Max.x = maxX;
        Max.y = maxY;
    }
}