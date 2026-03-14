using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core;

public interface IVertices
{
    Vector2[] CreateVertices();
    Vector2[] GetTransformedVertices();
}