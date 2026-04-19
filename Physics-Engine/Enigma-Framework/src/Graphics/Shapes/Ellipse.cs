using OpenTK.Graphics.OpenGL;

namespace Enigma_Framework.Graphics.Shapes;

public class Ellipse : Shape2D
{
    public int Segments = 24;

    public Ellipse()
    {
        ShapeRenderer.Instance.Renderables.Add(this);
    }

    public float Radius1 => Entity.Transform.WorldScale.x;
    public float Radius2 => Entity.Transform.WorldScale.y;

    ~Ellipse()
    {
        ShapeRenderer.Instance.Renderables.Remove(this);
    }

    public override void Draw()
    {
        GL.Color4(Color);
        DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
        DebugRenderer2D.DrawEllipse(Radius1, Radius2, Segments, Filled);
        DebugRenderer2D.Pop();
    }
}