using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Enigma_Framework.Graphics;

public class Camera2D
{
    public Vector2 Position = Vector2.Zero;
    public float Size = 100f;
    public float AspectRatio { get; private set; }
    public float Left { get; private set; }
    public float Right { get; private set; }
    public float Top { get; private set; }
    public float Bottom { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }

    public void Apply(int width, int height)
    {
        var aspect = width / (float)height;
        Width = width;
        Height = height;
        AspectRatio = aspect;
        Left = -Size * aspect;
        Right = Size * aspect;
        Bottom = -Size;
        Top = Size;
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(Left, Right,
            Bottom, Top, -1, 1);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        GL.Translate(-Position.X, -Position.Y, 0);
    }

    public Vector2 ScreenToWorldPoint(Vector2 screenSize, Vector2 screenPos)
    {
        var ndcX = screenPos.X / screenSize.X * 2f - 1f;
        var ndcY = 1f - screenPos.Y / screenSize.Y * 2f;

        var worldX = Left + (ndcX + 1f) * 0.5f * (Right - Left);
        var worldY = Bottom + (ndcY + 1f) * 0.5f * (Top - Bottom);

        worldX += Position.X;
        worldY += Position.Y;

        return new Vector2(worldX, worldY);
    }
}