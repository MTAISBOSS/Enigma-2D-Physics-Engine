using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics.Shapes
{
    public class Ellipse : Shape2D
    {
        public float Radius1 => Owner.Transform.Scale.x;
        public float Radius2 => Owner.Transform.Scale.y;
        public int Segments = 24;

        public Ellipse()
        {
            ShapeRenderer.Instance.Renderables.Add(this);
        }

        ~Ellipse()
        {
            ShapeRenderer.Instance.Renderables.Remove(this);
        }
        public override void Draw()
        {
            GL.Color4(Color);
            DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
            DebugRenderer2D.DrawEllipse(Radius1,Radius2, Segments, Filled);
            DebugRenderer2D.Pop();
        }
    }
}