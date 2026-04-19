using Physics_Engine.Graphics;

namespace Enigma_Framework.Graphics;

public class ShapeRenderer
{
    private static ShapeRenderer _instance;

    public readonly List<IRender> Renderables = new();
    private IRender[] debugElements;
    private IRender[] uiElements;
    private IRender[] worldElements;

    private ShapeRenderer()
    {
    }

    public static ShapeRenderer Instance
    {
        get
        {
            _instance ??= new ShapeRenderer();
            return _instance;
        }
    }

    public Camera2D MainCamera { get; set; }

    public void Render()
    {
        DebugRenderer2D.Begin();

        uiElements = Renderables.Where(o => o.GetLayerMask() == LayerMask.UI).ToArray();
        worldElements = Renderables.Where(o => o.GetLayerMask() == LayerMask.World).ToArray();
        debugElements = Renderables.Where(o => o.GetLayerMask() == LayerMask.Debug).ToArray();
        foreach (var r in worldElements)
            r.Draw();
        foreach (var r in uiElements)
            r.Draw();
        foreach (var r in debugElements)
            r.Draw();


        DebugRenderer2D.End();
    }

    public void SetCamera(Camera2D camera)
    {
        MainCamera = camera;
    }
}