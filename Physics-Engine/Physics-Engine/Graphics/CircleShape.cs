using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public class CircleShape : Shape2D
    {
        public float Radius = 1f;
        public int Segments = 24;

        public override void Draw()
        {
            GL.Color4(Color);
            DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
            DebugRenderer2D.DrawCircle(Radius, Segments, Filled);
            DebugRenderer2D.Pop();
        }
    }
}