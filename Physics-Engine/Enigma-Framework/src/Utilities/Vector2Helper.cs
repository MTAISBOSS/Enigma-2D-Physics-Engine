using Enigma_Framework.Math;

namespace Enigma_Framework.Utilities;

public static class Vector2Helper
{
    public static Vector2 ConvertToOpenTk(this OpenTK.Mathematics.Vector2 vector2)
    {
        return new Vector2(vector2.X, vector2.Y);
    }

    public static OpenTK.Mathematics.Vector2 ConvertFromOpenTk(this Vector2 vector2)
    {
        return new OpenTK.Mathematics.Vector2(vector2.x, vector2.y);
    }
}