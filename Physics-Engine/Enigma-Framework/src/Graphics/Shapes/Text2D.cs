using Enigma_Framework.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics.Shapes;

public enum TextAlignment
{
    Left,
    Center,
    Right
}

public class Text2D : Shape2D
{
    public TextAlignment Alignment = TextAlignment.Left;
    public string FontFamily = "Arial";
    public float FontSize = 12f;
    public string TextContent = "Sample Text";

    public Text2D()
    {
        ShapeRenderer.Instance.Renderables.Add(this);
    }

    ~Text2D()
    {
        ShapeRenderer.Instance.Renderables.Remove(this);
    }

    public override void Draw()
    {
        GL.Color4(Color);
        DebugRenderer2D.Push(Position.X, Position.Y, Rotation);
        DebugRenderer2D.DrawText(TextContent, FontFamily, FontSize, Alignment);
        DebugRenderer2D.Pop();
    }
}