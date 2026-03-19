using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public class Camera2D
    {
        public float Size = 50f;
        public Vector2 Position = Vector2.Zero;
        public float AspectRatio { get; private set; }
        public float Left { get; private set; }
        public float Right { get; private set; }
        public float Top { get; private set; }
        public float Bottom { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public void Apply(int width, int height)
        {
            float aspect = width / (float)height;
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
    }
}