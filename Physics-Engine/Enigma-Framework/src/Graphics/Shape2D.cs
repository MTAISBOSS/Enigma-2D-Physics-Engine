using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Transform;
using Enigma_Framework.Utilities;
using OpenTK.Mathematics;
using Physics_Engine.Graphics;

namespace Enigma_Framework.Graphics;

public abstract class Shape2D : Component, IRender
{
    public Color4 Color = Color4.White;

    public bool Filled = false;
    public LayerMask Layer = LayerMask.World;
    public int RenderOrder = 0;

    private Transform Transform
    {
        get
        {
            if (Entity != null) return Entity.Transform;

            return null;
        }
    }

    public virtual Vector2 Position => Transform.WorldPosition.ConvertFromOpenTk();
    public virtual Vector2 Scale => Transform.WorldScale.ConvertFromOpenTk();

    public virtual float Rotation
    {
        get
        {
            if (Transform != null) return Transform.WorldRotation;

            return 0;
        }
    }

    public int GetSortingOrder()
    {
        return RenderOrder;
    }

    public LayerMask GetLayerMask()
    {
        return Layer;
    }

    public abstract void Draw();

    public override bool IsAbleToDuplicate()
    {
        return false;
    }
}