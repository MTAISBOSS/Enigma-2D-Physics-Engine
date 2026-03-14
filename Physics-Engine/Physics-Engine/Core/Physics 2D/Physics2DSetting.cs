using Physics_Engine.Math;

namespace Physics_Engine.Core.Physics_2D
{
    public static class Physics2DSetting
    {
        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 64f * 64f;
        
        public static readonly float MinDensity  = 0.5f;
        public static readonly float MaxDensity = 21.4f;

        public static Vector2 GravityDirection = new Vector2(0f, -9.18f);

    }
}