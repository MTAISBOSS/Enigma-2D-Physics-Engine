using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Enigma_Framework.Core.LogSystem;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Physics_Engine.Graphics.Shapes;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace Enigma_Framework.Graphics;

public static class DebugRenderer2D
{
    private const float TextOversampleScale = 8.0f;

    private static readonly Dictionary<string, int> TextTextureCache = new();
    private static readonly Dictionary<string, SizeF> TextSizeCache = new();
    private static readonly PrivateFontCollection FontCollection = new();
    private static readonly Dictionary<string, FontFamily> LoadedFonts = new();

    public static void Begin()
    {
        GL.Disable(EnableCap.Texture2D);
        GL.Disable(EnableCap.Lighting);
        GL.Disable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
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

    public static void DrawRect(float w, float h, bool filled)
    {
        GL.Begin(filled ? PrimitiveType.Quads : PrimitiveType.LineLoop);

        GL.Vertex2(-w / 2, -h / 2);
        GL.Vertex2(w / 2, -h / 2);
        GL.Vertex2(w / 2, h / 2);
        GL.Vertex2(-w / 2, h / 2);

        GL.End();
    }

    public static void DrawCircle(float r, int segments, bool filled)
    {
        GL.Begin(filled ? PrimitiveType.TriangleFan : PrimitiveType.LineLoop);

        for (var i = 0; i < segments; i++)
        {
            var a = i / (float)segments * (float)System.Math.PI * 2f;
            GL.Vertex2(System.Math.Cos(a) * r, System.Math.Sin(a) * r);
        }

        GL.End();
    }

    public static void DrawEllipse(float r1, float r2, int segments, bool filled)
    {
        GL.Begin(filled ? PrimitiveType.TriangleFan : PrimitiveType.LineLoop);

        for (var i = 0; i < segments; i++)
        {
            var a = i / (float)segments * (float)System.Math.PI * 2f;
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

    public static void DrawText(string text, string fontName, float fontSize, TextAlignment alignment)
    {
        if (string.IsNullOrEmpty(text)) return;

        var cacheKey = $"{text}_{fontName}_{fontSize}";

        int textureId;
        float width, height;

        if (!TextTextureCache.ContainsKey(cacheKey))
        {
            textureId = GenerateTextTexture(text, fontName, fontSize, out width, out height);
            TextTextureCache[cacheKey] = textureId;
            TextSizeCache[cacheKey] = new SizeF(width, height);
        }
        else
        {
            textureId = TextTextureCache[cacheKey];
            width = TextSizeCache[cacheKey].Width;
            height = TextSizeCache[cacheKey].Height;
        }

        float offsetX = 0;
        var offsetY = -height / 2f;

        if (alignment == TextAlignment.Center)
            offsetX = -width / 2f;
        else if (alignment == TextAlignment.Right)
            offsetX = -width;

        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        GL.Begin(PrimitiveType.Quads);

        GL.TexCoord2(0.0f, 1.0f);
        GL.Vertex2(offsetX, offsetY);

        GL.TexCoord2(1.0f, 1.0f);
        GL.Vertex2(offsetX + width, offsetY);

        GL.TexCoord2(1.0f, 0.0f);
        GL.Vertex2(offsetX + width, offsetY + height);

        GL.TexCoord2(0.0f, 0.0f);
        GL.Vertex2(offsetX, offsetY + height);

        GL.End();

        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.Disable(EnableCap.Texture2D);
    }

    private static int GenerateTextTexture(string text, string fontName, float baseFontSize, out float visualWidth,
        out float visualHeight)
    {
        var highResFontSize = baseFontSize * TextOversampleScale;

        Font font;
        if (LoadedFonts.ContainsKey(fontName))
            font = new Font(LoadedFonts[fontName], highResFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        else
            font = new Font(fontName, highResFontSize, FontStyle.Regular, GraphicsUnit.Pixel);

        var dummy = new Bitmap(1, 1);
        var gfx = System.Drawing.Graphics.FromImage(dummy);

        gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        var highResSize = gfx.MeasureString(text, font);
        dummy.Dispose();
        gfx.Dispose();

        var bmpWidth = (int)System.Math.Ceiling(highResSize.Width);
        var bmpHeight = (int)System.Math.Ceiling(highResSize.Height);

        var bmp = new Bitmap(bmpWidth, bmpHeight);
        gfx = System.Drawing.Graphics.FromImage(bmp);
        gfx.Clear(Color.Transparent);
        gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        gfx.DrawString(text, font, Brushes.White, new PointF(0, 0));

        var textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

        bmp.UnlockBits(data);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
            (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
            (int)TextureWrapMode.ClampToEdge);

        gfx.Dispose();
        bmp.Dispose();
        font.Dispose();

        visualWidth = bmpWidth / TextOversampleScale;
        visualHeight = bmpHeight / TextOversampleScale;

        return textureId;
    }

    public static void LoadFont(string filePath)
    {
        try
        {
            FontCollection.AddFontFile(filePath);
            var family = FontCollection.Families[FontCollection.Families.Length - 1];
            LoadedFonts[family.Name] = family;
            Logger.Log($"Successfully loaded font: {family.Name}");
        }
        catch (Exception e)
        {
            Logger.Log($"Failed to load font at {filePath}: {e.Message}");
        }
    }
}