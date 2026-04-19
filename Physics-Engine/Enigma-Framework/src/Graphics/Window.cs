using Enigma_Framework.Core.Input;
using Enigma_Framework.Core.Time;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Enigma_Framework.Graphics;

public class Window
{
    private readonly GameWindow window;
    private readonly EnigmaCore core;

    public Window(GameWindow window)
    {
        this.window = window;
        this.core = new EnigmaCore();

        this.window.Load += OnLoad;
        this.window.RenderFrame += OnRender;
        this.window.UpdateFrame += OnUpdate;
        this.window.Resize += OnResize;
    }

    public void Run()
    {
        window.Run(); 
    }

    private void OnLoad()
    {
        core.Load(window.ClientSize.X, window.ClientSize.Y);
    }

    private void OnResize(ResizeEventArgs resizeEventArgs)
    {
        core.Resize(resizeEventArgs.Width, resizeEventArgs.Height);
    }

    private void OnUpdate(FrameEventArgs frameEventArgs)
    {
        InputSystem.Update(); 
        Time.Update(frameEventArgs);
        core.Update(frameEventArgs.Time);
    }

    private void OnRender(FrameEventArgs frameEventArgs)
    {
        core.Render(window.ClientSize.X, window.ClientSize.Y);
        window.SwapBuffers();
    }
}