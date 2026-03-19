using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Physics_Engine.Graphics
{
    public static class DebugRenderer2D
    {
        public static void Begin()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public static void End()
        {
            GL.Enable(EnableCap.DepthTest);
        }

        public static void Push(float x, float y, float rotDeg, float sx = 1, float sy = 1)
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0);
            GL.Rotate(rotDeg, 0, 0, 1);
            GL.Scale(sx, sy, 1);
        }

        public static void Pop()
        {
            GL.PopMatrix();
        }


        public static void DrawBox(float w, float h, bool filled)
        {
            GL.Begin(filled ? PrimitiveType.Quads : PrimitiveType.LineLoop);

            GL.Vertex2(-w / 2, -h / 2);
            GL.Vertex2( w / 2, -h / 2);
            GL.Vertex2( w / 2,  h / 2);
            GL.Vertex2(-w / 2,  h / 2);

            GL.End();
        }
        public static void DrawCircle(float r, int segments, bool filled)
        {
            GL.Begin(filled ? PrimitiveType.TriangleFan : PrimitiveType.LineLoop);

            for (int i = 0; i < segments; i++)
            {
                float a = i / (float)segments * (float)System.Math.PI * 2f;
                GL.Vertex2(System.Math.Cos(a) * r, System.Math.Sin(a) * r);
            }

            GL.End();
        }
        public static void DrawEllipse(float r1,float r2, int segments, bool filled)
        {
            GL.Begin(filled ? PrimitiveType.TriangleFan : PrimitiveType.LineLoop);

            for (int i = 0; i < segments; i++)
            {
                float a = i / (float)segments * (float)System.Math.PI * 2f;
                GL.Vertex2(System.Math.Cos(a) * r1, System.Math.Sin(a) * r2);
            }

            GL.End();
        }
        public static void DrawPolygon(Vector2[] verts, bool filled)
        {
            GL.Begin(filled ? PrimitiveType.Polygon : PrimitiveType.LineLoop);

            foreach (var v in verts)
                GL.Vertex2(v.X, v.Y);

            GL.End();
        }
        public static void DrawLine(Vector2 a, Vector2 b)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(a);
            GL.Vertex2(b);
            GL.End();
        }
    }
}