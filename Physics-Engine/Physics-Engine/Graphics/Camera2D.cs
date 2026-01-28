using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public class Camera2D
    {
        public float Size = 50f;
        public Vector2 Position = Vector2.Zero;

        public void Apply(int width, int height)
        {
            float aspect = width / (float)height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-Size * aspect, Size * aspect,
                -Size, Size, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(-Position.X, -Position.Y, 0);
        }
    }
}