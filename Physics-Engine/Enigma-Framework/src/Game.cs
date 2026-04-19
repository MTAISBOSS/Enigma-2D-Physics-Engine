using System.Drawing.Imaging;
using Enigma_Framework.Audio;
using Enigma_Framework.Core.AssetPipeline;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Input;
using Enigma_Framework.Core.LogSystem;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Graphics;
using Enigma_Framework.Utilities;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace Enigma_Framework;

public class Game
{
    private GameWindow _game;
    private Window _window;
    private bool isRunning;

    public Game()
    {
        isRunning = false;
        Logger.LogWarning("Game constructor called.");
    }

    ~Game()
    {
        Logger.LogWarning("Game destructor called.");
    }

    public void Initialize()
    {
        var nativeSettings = new NativeWindowSettings
        {
            Title = "Enigma Engine",
            Profile = ContextProfile.Compatability,
            Flags = ContextFlags.Default,
            Size = new Vector2i(800, 800)
        };
        using var game = new GameWindow(GameWindowSettings.Default, nativeSettings);
        _window = new Window(game);

        isRunning = true;
        
        InputSystem.Initialize(_game);

        AudioManager.Initialize();

        ComponentRegistry.Initialize();

        PhysicsContext.Initialize();
    }

    public void ProcessInput()
    {
        
    }

    public void Update()
    {
    }

    public void Render()
    {
    }

    public void Run()
    {
        if (!isRunning)
        {
            return;
        }

        _window.Run();
    }

    public void Destroy()
    {
        _game.Dispose();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}