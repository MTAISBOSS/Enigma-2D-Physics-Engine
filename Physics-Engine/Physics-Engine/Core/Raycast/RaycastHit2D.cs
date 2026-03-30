using Physics_Engine.Core.Collision;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Raycast;

public struct RaycastHit2D
{
    public Collider Collider;
    public Vector2 Point;
    public Vector2 Normal;
    public float Distance;
}