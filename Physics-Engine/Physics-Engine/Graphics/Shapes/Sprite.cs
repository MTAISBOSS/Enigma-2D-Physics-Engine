using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using System.Drawing;
using System.Drawing.Imaging;

namespace Physics_Engine.Graphics.Shapes
{
    public class Sprite : Shape2D
    {
        public int TextureId { get; private set; }
        public float Width => Owner.Transform.Scale.x;
        public float Height => Owner.Transform.Scale.y;

        public Sprite(string filePath)
        {
            TextureId = LoadTexture(filePath);
            ShapeRenderer.Instance.Renderables.Add(this);
        }

        public Sprite(int textureId)
        {
            TextureId = textureId;
            ShapeRenderer.Instance.Renderables.Add(this);
        }

        ~Sprite()
        {
            ShapeRenderer.Instance.Renderables.Remove(this);
            if (TextureId != 0)
                GL.DeleteTexture(TextureId);
        }

        public override void Draw()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);

            GL.Color4(Color);

            DebugRenderer2D.Push(Position.X, Position.Y, Rotation, Width, Height);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0); GL.Vertex2(-0.5f, -0.5f);
            GL.TexCoord2(1, 0); GL.Vertex2( 0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex2( 0.5f,  0.5f);
            GL.TexCoord2(0, 1); GL.Vertex2(-0.5f,  0.5f);

            GL.End();

            DebugRenderer2D.Pop();

            GL.Disable(EnableCap.Texture2D);
        }

        private int LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            using (Bitmap bmp = new Bitmap(path))
            {
                BitmapData data = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    data.Width,
                    data.Height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0
                );

                bmp.UnlockBits(data);
            }
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
/*
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
*/
            return id;
        }
    }
}
