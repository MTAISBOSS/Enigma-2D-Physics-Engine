using Enigma_Framework.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics.Shapes;

public class Rectangle : Shape2D
{
    public Rectangle()
    {
        ShapeRenderer.Instance.Renderables.Add(this);
    }

    public float Width => Entity.Transform.WorldScale.x;
    public float Height => Entity.Transform.WorldScale.y;

    ~Rectangle()
    {
        ShapeRenderer.Instance.Renderables.Remove(this);
    }

    public override void Draw()
    {
        GL.Color4(Color);
        DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
        DebugRenderer2D.DrawRect(Width, Height, Filled);
        DebugRenderer2D.Pop();
    }
}