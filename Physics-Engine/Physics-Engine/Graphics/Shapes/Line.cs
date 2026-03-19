using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics.Shapes
{
    public class Line : Shape2D
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public override void Draw()
        {
            GL.Color4(Color);
            var center =(StartPosition + EndPosition) / 2;
            DebugRenderer2D.Push(center.X,center.Y,Rotation);
            DebugRenderer2D.DrawLine(StartPosition,EndPosition);
            DebugRenderer2D.Pop();
        }
    }
}