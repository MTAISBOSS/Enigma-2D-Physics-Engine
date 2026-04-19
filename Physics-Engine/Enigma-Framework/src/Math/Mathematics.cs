namespace Enigma_Framework.Math;

public struct Mathematics
{
    public static float Length(Vector2 a)
    {
        return (float)System.Math.Sqrt(System.Math.Pow(a.x, 2) + System.Math.Pow(a.y, 2));
    }

    public static float Distance(Vector2 a, Vector2 b)
    {
        return (float)System.Math.Sqrt(System.Math.Pow(a.x - b.x, 2) + System.Math.Pow(a.y - b.y, 2));
    }

    public static Vector2 Normalize(Vector2 a)
    {
        return a / Length(a);
    }

    public static float DotProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float CrossProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static float Clamp(float current, float min, float max)
    {
        if (current <= min) return min;

        if (current >= max) return max;

        return current;
    }

    public static float DistanceSq(Vector2 a, Vector2 b)
    {
        return (float)(System.Math.Pow(a.x - b.x, 2) + System.Math.Pow(a.y - b.y, 2));
    }

    public static float LengthSq(Vector2 a)
    {
        return (float)(System.Math.Pow(a.x, 2) + System.Math.Pow(a.y, 2));
    }

    public static float Sqrt(float scalar)
    {
        return (float)System.Math.Sqrt(scalar);
    }

    public static bool IsNearlyEqual(float a, float b)
    {
        return System.Math.Abs(a - b) < 0.0005f;
    }

    public static bool IsNearlyEqual(Vector2 a, Vector2 b)
    {
        return System.Math.Abs(a.x - b.x) < 0.0005f && System.Math.Abs(a.y - b.y) < 0.0005f;
    }
}