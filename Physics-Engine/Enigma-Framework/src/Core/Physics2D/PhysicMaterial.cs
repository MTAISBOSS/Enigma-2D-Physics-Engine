namespace Enigma_Framework.Core.Physics2D;

public struct PhysicMaterial
{
    public readonly float StaticFriction;
    public readonly float DynamicFriction;
    public readonly float Restitution;

    public PhysicMaterial(float staticFriction, float dynamicFriction, float restitution)
    {
        StaticFriction = staticFriction;
        DynamicFriction = dynamicFriction;
        Restitution = restitution;
    }
}