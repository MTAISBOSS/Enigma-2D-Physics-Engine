using System.Collections.Generic;
using System.Linq;

namespace Physics_Engine.Graphics
{
    public class ShapeRenderer
    {
        public readonly List<IRender> Renderables = new List<IRender>();

        public void Render()
        {
            DebugRenderer2D.Begin();

            foreach (var r in Renderables.OrderBy(r => r.GetSortingOrder()))
                r.Draw();

            DebugRenderer2D.End();
        }
    }
}