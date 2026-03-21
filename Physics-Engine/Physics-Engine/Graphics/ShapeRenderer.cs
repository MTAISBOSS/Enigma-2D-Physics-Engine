using System.Collections.Generic;
using System.Linq;
using Physics_Engine.Core;

namespace Physics_Engine.Graphics
{
    public class ShapeRenderer
    {
        private static ShapeRenderer _instance;
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

        private ShapeRenderer() { }

        public void Render()
        {
            DebugRenderer2D.Begin();
            
            foreach (var r in Renderables.OrderBy(r => r.GetSortingOrder()))
                r.Draw();

            DebugRenderer2D.End();
        }

        public void SetCamera(Camera2D camera)
        {
            MainCamera = camera;
        }

    }
}