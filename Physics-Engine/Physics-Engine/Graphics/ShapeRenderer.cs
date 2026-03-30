using System.Collections.Generic;
using System.Linq;
using Physics_Engine.Core;

namespace Physics_Engine.Graphics
{
    public class ShapeRenderer
    {
        private static ShapeRenderer _instance;
        private IRender[] uiElements;
        private IRender[] worldElements;
        private IRender[] debugElements;
        public static ShapeRenderer Instance
        {
            get
            {
                _instance ??= new ShapeRenderer();
                return _instance;
            }
        }

        public readonly List<IRender> Renderables = new List<IRender>();
        public Camera2D MainCamera { get; set; }

        private ShapeRenderer()
        {
        }

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
}