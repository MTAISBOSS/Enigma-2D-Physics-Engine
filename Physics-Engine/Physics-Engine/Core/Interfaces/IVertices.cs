using Physics_Engine.Math;

namespace Physics_Engine.Core.Interfaces;

public interface IVertices
{
    Vector2[] CreateVertices();
    Vector2[] GetTransformedVertices();
}