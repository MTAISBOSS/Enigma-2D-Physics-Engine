using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Interfaces;

public interface IMovement
{
    void MoveToExactPosition(Vector2 position);
    void MoveByAmount(Vector2 amount);
}