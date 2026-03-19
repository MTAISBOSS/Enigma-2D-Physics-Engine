using Physics_Engine.Core.Component_System;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Transform
{
    public class Transform2D : ComponentBase
    {
        public Vector2 Position { get; set; } = new(0, 0);
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = new(1, 1);

        public void Translate(Vector2 delta)
        {
            Position += delta;
        }

        public void Rotate(float deltaAngle)
        {
            Rotation += deltaAngle;
        }

        public override bool IsAbleToDuplicate() => false;
    }
}