using Enigma_Framework.Core.ECS;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Transform;

public class Transform : Component
{
    public Transform? Parent { get; private set; }
    public IReadOnlyList<Transform> Children => _children;
    private readonly List<Transform> _children = new();
    
    public Vector2 WorldPosition
    {
        get
        {
            if (Parent == null)
                return LocalPosition;

            return Parent.WorldPosition + LocalPosition;
        }
        set
        {
            if (Parent == null)
                LocalPosition = value;
            else
                LocalPosition = value - Parent.WorldPosition;
        }
    }


    public Vector2 WorldScale
    {
        get
        {
            if (Parent == null)
                return LocalScale;

            return Parent.WorldScale * LocalScale;
        }
        set
        {
            if (Parent == null)
                LocalScale = value;
            else
                LocalScale = value / Parent.WorldScale;
        }
    }

    public float WorldRotation
    {
        get
        {
            if (Parent == null)
                return LocalRotation;

            return Parent.WorldRotation + LocalRotation;
        }
        set
        {
            if (Parent == null)
                LocalRotation = value;
            else
                LocalRotation = value - Parent.WorldRotation;
        }
    }



    public Vector2 LocalPosition { get; set; } = new(0, 0);
    public Vector2 LocalScale { get; set; } = Vector2.One;
    public float LocalRotation { get; set; }

    public void SetParent(Transform parent)
    {
        if (parent == this)
            return;

        Parent?._children.Remove(this);

        Parent = parent;

        parent._children.Add(this);
    }
    public void RemoveParent()
    {
        Parent?._children.Remove(this);
        Parent = null;
    }



    public Transform? GetChild(int index)
    {
        if (index< Children.Count && index >=0)
        {
            return Children[index];
        }

        return null;
    }
    public void TranslateLocal(Vector2 delta)
    {
        LocalPosition += delta;
    }

    public void TranslateWorld(Vector2 delta)
    {
        WorldPosition += delta;
    }
    public void Rotate(float deltaAngle)
    {
        WorldRotation += deltaAngle;
    }

    public override bool IsAbleToDuplicate()
    {
        return false;
    }
}