using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public class BoxShape : Shape2D
    {
        public float Width = 1;
        public float Height = 1;

        public override void Draw()
        {
            GL.Color4(Color);
            DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
            DebugRenderer2D.DrawBox(Width, Height, Filled);
            DebugRenderer2D.Pop();
        }
    }
}