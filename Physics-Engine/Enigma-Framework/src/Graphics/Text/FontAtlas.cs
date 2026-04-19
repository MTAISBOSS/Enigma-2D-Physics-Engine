using System;
using System.Collections.Generic;
using System.IO;
using StbTrueTypeSharp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class FontGlyph
{
    public float X0, Y0, X1, Y1;
    public float XOffset, YOffset;
    public float XAdvance;
}

public class FontAtlas
{
    public int TextureID;
    public int AtlasWidth;
    public int AtlasHeight;

    public Dictionary<char, FontGlyph> Glyphs = new();

    public float Ascent;
    public float Descent;
    public float LineGap;
    public float FontSize;

    // 1. Added 'unsafe' keyword because StbTrueType uses raw pointers
       public unsafe FontAtlas(string ttfPath, float fontSize = 32f)
    {
        FontSize = fontSize;

        byte[] ttf = File.ReadAllBytes(ttfPath);
        var stbFont = new StbTrueType.stbtt_fontinfo();

        // Pin the managed array in memory to get a raw pointer (byte*)
        fixed (byte* pTtf = ttf)
        {
            // Pass the pointer instead of the array
            if (StbTrueType.stbtt_InitFont(stbFont, pTtf, 0) == 0)
                throw new Exception("Failed to init font.");

            int ascent, descent, lineGap;
            StbTrueType.stbtt_GetFontVMetrics(stbFont, &ascent, &descent, &lineGap);

            float scale = StbTrueType.stbtt_ScaleForPixelHeight(stbFont, fontSize);
            Ascent = ascent * scale;
            Descent = descent * scale;
            LineGap = lineGap * scale;

            AtlasWidth = 512;
            AtlasHeight = 512;

            byte[] atlas = new byte[AtlasWidth * AtlasHeight];

            int x = 0, y = 0, rowHeight = 0;

            // ASCII glyph set
            for (char c = (char)32; c < (char)128; c++)
            {
                int advance, bearing;
                StbTrueType.stbtt_GetCodepointHMetrics(stbFont, c, &advance, &bearing);

                int x0, y0, x1, y1;
                StbTrueType.stbtt_GetCodepointBitmapBox(stbFont, c, scale, scale, &x0, &y0, &x1, &y1);

                int gw = x1 - x0;
                int gh = y1 - y0;

                if (x + gw >= AtlasWidth)
                {
                    x = 0;
                    y += rowHeight + 1;
                    rowHeight = 0;
                }

                int xoff, yoff;
                byte* bmp = StbTrueType.stbtt_GetCodepointBitmap(
                    stbFont, 0, scale, c, &gw, &gh, &xoff, &yoff
                );

                if (bmp != null)
                {
                    for (int row = 0; row < gh; row++)
                    {
                        for (int col = 0; col < gw; col++)
                        {
                            atlas[(x + col) + (y + row) * AtlasWidth] = bmp[col + row * gw];
                        }
                    }

                    StbTrueType.stbtt_FreeBitmap(bmp, null);
                }

                Glyphs[c] = new FontGlyph
                {
                    X0 = x / (float)AtlasWidth,
                    Y0 = y / (float)AtlasHeight,
                    X1 = (x + gw) / (float)AtlasWidth,
                    Y1 = (y + gh) / (float)AtlasHeight,
                    XOffset = x0,
                    YOffset = y0,
                    XAdvance = advance * scale
                };

                x += gw + 1;
                rowHeight = Math.Max(rowHeight, gh);
            }

            // Upload atlas to OpenGL
            GL.CreateTextures(TextureTarget.Texture2D, 1, out TextureID);
            GL.TextureStorage2D(TextureID, 1, SizedInternalFormat.R8, AtlasWidth, AtlasHeight);

            GL.TextureSubImage2D(TextureID, 0, 0, 0, AtlasWidth, AtlasHeight,
                PixelFormat.Red, PixelType.UnsignedByte, atlas);

            GL.TextureParameter(TextureID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(TextureID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameter(TextureID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TextureParameter(TextureID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        } // The 'fixed' block ends here, unpinning the array safely because we are done reading from it.
    }

}
