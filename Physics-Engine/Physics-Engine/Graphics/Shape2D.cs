using OpenTK;
using OpenTK.Graphics;
using Physics_Engine.Core;
using Physics_Engine.Core.Physics_Engine.Core;

namespace Physics_Engine.Graphics
{
    public abstract class Shape2D : ComponentBase,IRender
    {
        public Vector2 Position;
        public float Rotation;

        public bool Filled = false;

        public Color4 Color = Color4.White;
        public int Layer = 0;

        public int GetSortingOrder()
        {
            return Layer;
        }

        public abstract void Draw();
        
        public override bool IsAbleToBeCloned()
        {
            return false;
        }
    }
}