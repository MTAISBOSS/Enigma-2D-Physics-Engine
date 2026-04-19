using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public struct AABBCollision
{
    public readonly Vector2 Min;
    public readonly Vector2 Max;

    public AABBCollision(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public AABBCollision(float minX, float minY, float maxX, float maxY)
    {
        Min = new Vector2(minX, minY);
        Max = new Vector2(maxX, maxY);
    }


    public static AABBCollision Combine(AABBCollision colA, AABBCollision colB)
    {
        var minX = System.Math.Min(colA.Min.x, colB.Min.x);
        var minY = System.Math.Min(colA.Min.y, colB.Min.y);
        var maxX = System.Math.Max(colA.Max.x, colB.Max.x);
        var maxY = System.Math.Max(colA.Max.y, colB.Max.y);

        return new AABBCollision(minX, minY, maxX, maxY);
    }
}