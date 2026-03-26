namespace Physics_Engine.Core.Physics_2D;

public struct PhysicMaterial
{
    public readonly float Bounciness ;
    public readonly float StaticFriction ;
    public readonly float DynamicFriction ;
    public readonly float Restitution ;

    public PhysicMaterial(float bounciness, float staticFriction, float dynamicFriction, float restitution)
    {
        Bounciness = bounciness;
        StaticFriction = staticFriction;
        DynamicFriction = dynamicFriction;
        Restitution = restitution;
    }
}