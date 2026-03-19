using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics.Shapes
{
    public class Polygon : Shape2D
    {
        public Vector2[] Vertices;

        public Polygon()
        {
            ShapeRenderer.Instance.Renderables.Add(this);
        }

        ~Polygon()
        {
            ShapeRenderer.Instance.Renderables.Remove(this);
        }
        public override void Draw()
        {
            GL.Color4(Color);
            DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
            DebugRenderer2D.DrawPolygon(Vertices, Filled);
            DebugRenderer2D.Pop();
        }
    }
}