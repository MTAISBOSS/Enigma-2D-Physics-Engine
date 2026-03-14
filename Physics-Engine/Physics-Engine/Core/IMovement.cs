using Physics_Engine.Math;

namespace Physics_Engine.Core
{
    public interface IMovement
    {
        void MoveTowards(Vector2 position);
        void Move(Vector2 amount);
    }
}