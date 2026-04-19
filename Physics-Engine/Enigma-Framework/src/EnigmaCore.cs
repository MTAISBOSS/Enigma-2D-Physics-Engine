using Enigma_Framework.Core.Input;
using Enigma_Framework.Core.Time;
using Enigma_Framework.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Enigma_Framework;

public class EnigmaCore
{
    private Camera2D camera;
    private ShapeRenderer shapeRenderer;

    public void Load(int width, int height)
    {
        GL.ClearColor(Color4.DimGray);
        GL.Disable(EnableCap.DepthTest);
        
        camera = new Camera2D();
        shapeRenderer = ShapeRenderer.Instance;
        shapeRenderer.SetCamera(camera);
        
        shapeRenderer.MainCamera.Apply(width, height);
        
        Time.Initialize(0.0);
        Engine.Start();
    }

    public void Resize(int width, int height)
    {
        if (width <= 0 || height <= 0) return; 
        
        GL.Viewport(0, 0, width, height);
    }

    public void Update(double deltaTime)
    {
        Engine.Update();
    }

    public void Render(int width, int height)
    {
        if (width <= 0 || height <= 0) return;

        GL.Clear(ClearBufferMask.ColorBufferBit);
        shapeRenderer.MainCamera.Apply(width, height);
        shapeRenderer.Renderables.ForEach(o => o.Draw());
        shapeRenderer.Render();
    }
    public void SetKeyDown(Keys keycode)
    {
        InputSystem.IsKeyDown(keycode); 
    }

    public void SetKeyUp(Keys openTkKey)
    {
        InputSystem.IsKeyUp(openTkKey); 
    }

    public void SetMousePosition(double positionX, double glY)
    {
        InputSystem.SetMousePosition(positionX,glY);
    }

    public void SetMouseDown(MouseButton eChangedButton)
    {
        
    }

    public void SetMouseUp(MouseButton eChangedButton)
    {
        
    }
}