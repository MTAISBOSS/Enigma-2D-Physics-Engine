using Physics_Engine.Math;

namespace Physics_Engine.Core.Interfaces
{
    public interface IMovement
    {
        void MoveToExactPosition(Vector2 position);
        void MoveByAmount(Vector2 amount);
    }
}