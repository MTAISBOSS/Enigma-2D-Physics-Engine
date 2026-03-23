using OpenTK.Graphics;
using Physics_Engine.Core.Component_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Transform;
using Physics_Engine.Utilities;
using Vector2 = OpenTK.Vector2;

namespace Physics_Engine.Graphics;

public abstract class Shape2D : Component, IRender
{
    private Transform Transform => Entity.Transform;

    public bool Filled = false;
    public Color4 Color = Color4.White;
    public int RenderOrder = 0;
    public LayerMask Layer;
    public virtual Vector2 Position => Transform.Position.ConvertFromOpenTk();
    public virtual Vector2 Scale => Transform.Scale.ConvertFromOpenTk();
    public virtual float Rotation => Transform.Rotation;

    public int GetSortingOrder()
    {
        return RenderOrder;
    }

    public abstract void Draw();

    public override bool IsAbleToDuplicate() => false;
}