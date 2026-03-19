using Physics_Engine.Math;

namespace Physics_Engine.Utilities
{
    public static class HelperExtensions
    {
        public static Vector2 ConvertToOpenTk(this OpenTK.Vector2 vector2)
        {
            return new Vector2(vector2.X, vector2.Y);
        }
        public static OpenTK.Vector2 ConvertFromOpenTk(this Vector2 vector2)
        {
            return new OpenTK.Vector2(vector2.x, vector2.y);
        }
    }
}