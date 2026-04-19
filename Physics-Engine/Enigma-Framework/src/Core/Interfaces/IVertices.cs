using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Interfaces;

public interface IVertices
{
    Vector2[] CreateVertices();
    Vector2[] GetTransformedVertices();
}