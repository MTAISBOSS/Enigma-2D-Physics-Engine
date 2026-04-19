using OpenTK.Graphics.OpenGL;

namespace Enigma_Framework.Graphics.Shapes;

public class Circle : Shape2D
{
    public float Radius = 1f;
    public int Segments = 24;

    public Circle()
    {
        ShapeRenderer.Instance.Renderables.Add(this);
    }

    ~Circle()
    {
        ShapeRenderer.Instance.Renderables.Remove(this);
    }

    public override void Draw()
    {
        GL.Color4(Color);
        DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
        DebugRenderer2D.DrawCircle(Radius, Segments, Filled);
        DebugRenderer2D.Pop();
    }
}