using Physics_Engine.Math;

namespace Physics_Engine.Core.Physics_2D
{
    public struct PhysicsSetting
    {
        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 640f * 640f;

        public static readonly float MinDensity = 0.1f;
        public static readonly float MaxDensity = 21.4f;

        public static Vector2 GravityDirection = new Vector2(0f, 2*-9.81f);

        public static int MinIterations = 1;
        public static int MaxIterations = 128;
        public static int Iterations = 2;
        //Mass is in the kg 
        public static float MaxMass = 2000;
        public static float MinMass = 0.001f;
    }
}