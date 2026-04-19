using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Physics2D;

public struct PhysicsSetting
{
    public static Vector2 GravityDirection = new(0f, -9.81f);

    public static int MinIterations = 1;
    public static int MaxIterations = 128;

    public static int Iterations = 4;

    //Mass is in the kg 
    public static float MaxMass = 2000;
    public static float MinMass = 0.001f;
}