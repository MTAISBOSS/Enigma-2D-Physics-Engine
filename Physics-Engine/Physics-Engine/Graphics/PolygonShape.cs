using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public class PolygonShape : Shape2D
    {
        public Vector2[] Vertices;

        public override void Draw()
        {
            GL.Color4(Color);
            DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
            DebugRenderer2D.DrawPolygon(Vertices, Filled);
            DebugRenderer2D.Pop();
        }
    }
}