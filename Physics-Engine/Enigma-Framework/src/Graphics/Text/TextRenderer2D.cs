using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Physics_Engine.Graphics.Shapes;

namespace Enigma_Framework.Graphics.Text;

public static class TextRenderer2D
{
    private static FontAtlas? _defaultAtlas;
    private static int _shader;
    private static int _vao, _vbo;

    public static void Initialize(string ttfPath)
    {
        _defaultAtlas = new FontAtlas(ttfPath, 32f);

        _shader = CreateShader();
        CreateBuffers();
    }

    private static void CreateBuffers()
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
    }

    public static void DrawText(string text, float x, float y, Color4 color, TextAlignment alignment)
    {
        if (string.IsNullOrEmpty(text)) return;

        GL.UseProgram(_shader);
        GL.BindVertexArray(_vao);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _defaultAtlas.TextureID);

        GL.Uniform4(GL.GetUniformLocation(_shader, "uColor"), color);

        
        float totalWidth = 0;
        foreach (char c in text)
            if (_defaultAtlas.Glyphs.TryGetValue(c, out var g))
                totalWidth += g.XAdvance;

        if (alignment == TextAlignment.Center)
            x -= totalWidth / 2f;
        else if (alignment == TextAlignment.Right)
            x -= totalWidth;

        foreach (char c in text)
        {
            if (!_defaultAtlas.Glyphs.TryGetValue(c, out var g))
                continue;

            float x0 = x + g.XOffset;
            float y0 = y + g.YOffset;
            float w = (g.X1 - g.X0) * _defaultAtlas.AtlasWidth;
            float h = (g.Y1 - g.Y0) * _defaultAtlas.AtlasHeight;

            float[] verts =
            {
                x0,     y0,     g.X0, g.Y0,
                x0+w,   y0,     g.X1, g.Y0,
                x0+w,   y0+h,   g.X1, g.Y1,

                x0,     y0,     g.X0, g.Y0,
                x0+w,   y0+h,   g.X1, g.Y1,
                x0,     y0+h,   g.X0, g.Y1,
            };

            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, verts.Length * sizeof(float), verts);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            x += g.XAdvance;
        }
    }

    private static int CreateShader()
    {
        string vs = @"
#version 330 core
layout(location = 0) in vec2 aPos;
layout(location = 1) in vec2 aUV;

out vec2 vUV;

uniform mat4 uProjection;

void main()
{
    vUV = aUV;
    gl_Position = uProjection * vec4(aPos, 0.0, 1.0);
}";

        string fs = @"
#version 330 core
in vec2 vUV;
out vec4 FragColor;

uniform sampler2D uTex;
uniform vec4 uColor;

void main()
{
    float alpha = texture(uTex, vUV).r;
    FragColor = vec4(uColor.rgb, uColor.a * alpha);
}";

        int v = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(v, vs);
        GL.CompileShader(v);

        int f = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(f, fs);
        GL.CompileShader(f);

        int s = GL.CreateProgram();
        GL.AttachShader(s, v);
        GL.AttachShader(s, f);
        GL.LinkProgram(s);

        GL.DeleteShader(v);
        GL.DeleteShader(f);

        return s;
    }
}