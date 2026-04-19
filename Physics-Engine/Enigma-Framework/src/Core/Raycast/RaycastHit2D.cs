using Enigma_Framework.Core.Collision;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Raycast;

public struct RaycastHit2D
{
    public Collider Collider;
    public Vector2 Point;
    public Vector2 Normal;
    public float Distance;
}