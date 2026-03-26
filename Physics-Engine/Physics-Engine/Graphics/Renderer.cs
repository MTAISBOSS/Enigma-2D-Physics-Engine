using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Vector2 = Physics_Engine.Math.Vector2;


namespace Physics_Engine.Graphics
{
    public class Renderer
    {
        private GameWindow _window;

        public void SetCurrentWindowContext(GameWindow window)
        {
            _window = window;
            window.Load += WindowOnLoad;
            window.RenderFrame += WindowOnRenderFrame;
            window.Resize += WindowOnResize;
        }

        private void WindowOnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _window.Width, _window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-50, 50, -50, 50, -100, 100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void WindowOnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _window.SwapBuffers();
        }

        public void PrepareTexture(int texture, string texturePath)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.ColorMaterial);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            BitmapData image = LoadTexture($@"{texturePath}");
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, image.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        private void DrawQuad()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.Begin(PrimitiveType.Quads);

            GL.Color4(0.0, 0.0, 1.0, 0.6);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(5.0, 0.0, 0.0);
            GL.Vertex3(5.0, 5.0, 0.0);
            GL.Vertex3(0.0, 5.0, 0.0);

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(1.0, 1.0, 1.0);

            float size = 10f;

            // FRONT
            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-size, +size, +size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(+size, +size, +size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(+size, -size, +size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-size, -size, +size);

            // LEFT
            GL.Normal3(-1, 0, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-size, +size, -size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-size, +size, +size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(-size, -size, +size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-size, -size, -size);

            // BACK
            GL.Normal3(0, 0, -1);
            GL.TexCoord2(0, 0);
            GL.Vertex3(+size, +size, -size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-size, +size, -size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(+size, -size, -size);

            // RIGHT
            GL.Normal3(1, 0, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(+size, +size, +size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(+size, +size, -size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(+size, -size, -size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(+size, -size, +size);

            // TOP
            GL.Normal3(0, 1, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-size, +size, -size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(+size, +size, -size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(+size, +size, +size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-size, +size, +size);

            // BOTTOM
            GL.Normal3(0, -1, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-size, -size, +size);
            GL.TexCoord2(1, 0);
            GL.Vertex3(+size, -size, +size);
            GL.TexCoord2(1, 1);
            GL.Vertex3(+size, -size, -size);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-size, -size, -size);

            GL.End();
        }

        private void DrawTexturedSphere(float radius, int slices, int stacks, int textureId)
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < stacks; i++)
            {
                float phi0 = (float)(System.Math.PI * i / stacks);
                float phi1 = (float)(System.Math.PI * (i + 1) / stacks);

                for (int j = 0; j < slices; j++)
                {
                    float theta0 = (float)(2 * System.Math.PI * j / slices);
                    float theta1 = (float)(2 * System.Math.PI * (j + 1) / slices);

                    float u0 = j / (float)slices;
                    float u1 = (j + 1) / (float)slices;
                    float v0 = i / (float)stacks;
                    float v1 = (i + 1) / (float)stacks;

                    var v00 = GetSpherePoint(radius, phi0, theta0);
                    var v01 = GetSpherePoint(radius, phi0, theta1);
                    var v11 = GetSpherePoint(radius, phi1, theta1);
                    var v10 = GetSpherePoint(radius, phi1, theta0);

                    var n00 = Vector3.Normalize(v00);
                    var n01 = Vector3.Normalize(v01);
                    var n11 = Vector3.Normalize(v11);
                    var n10 = Vector3.Normalize(v10);

                    GL.TexCoord2(u0, v0);
                    GL.Normal3(n00);
                    GL.Vertex3(v00);

                    GL.TexCoord2(u1, v0);
                    GL.Normal3(n01);
                    GL.Vertex3(v01);

                    GL.TexCoord2(u1, v1);
                    GL.Normal3(n11);
                    GL.Vertex3(v11);

                    GL.TexCoord2(u0, v1);
                    GL.Normal3(n10);
                    GL.Vertex3(v10);
                }
            }

            GL.End();
        }

        private Vector3 GetSpherePoint(float radius, float phi, float theta)
        {
            float x = (float)(radius * System.Math.Sin(phi) * System.Math.Cos(theta));
            float y = (float)(radius * System.Math.Cos(phi));
            float z = (float)(radius * System.Math.Sin(phi) * System.Math.Sin(theta));
            return new Vector3(x, y, z);
        }

        private void WindowOnLoad(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        BitmapData LoadTexture(string filePath)
        {
            Bitmap bmp = new Bitmap(filePath);
            var rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData =
                bmp.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            bmp.UnlockBits(bmpData);
            return bmpData;
        }

        public void DrawTriangle(Vector2 position, float rotation, Vector2 scale, Color4 color)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector2 firstPoint = position - Vector2.Right;
            Vector2 secondPoint = position + Vector2.Up;
            Vector2 thirdPoint = position + Vector2.Right;
            
            GL.Begin(PrimitiveType.Triangles);
            
            GL.Color4(color);
            GL.Translate(position.x, position.y, 0);
            GL.Rotate(rotation, 0.0, 0.0, 1.0);
            GL.Scale(scale.x, scale.y, 1.0);
            
            GL.Vertex2(firstPoint.x,firstPoint.y);
            GL.Vertex2(secondPoint.x,secondPoint.y);
            GL.Vertex2(thirdPoint.x,thirdPoint.y);

            GL.End();
            _window.SwapBuffers();
        }
    }
}