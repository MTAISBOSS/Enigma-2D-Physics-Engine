using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Interfaces;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Rigidbody;

public partial class Rigidbody2D : Component, IMovement, IRotation
{
    private Vector2 force = Vector2.Zero;
    private Enigma_Framework.Core.Transform.Transform Transform => Entity.Transform;
    public RigidbodyData Body { get; } = new();

    public float Rotation
    {
        get => Transform.WorldRotation;
        private set
        {
            Transform.WorldRotation = value;
            UpdateColliderCaches();
        }
    }

    public Vector2 Position
    {
        get => Transform.WorldPosition;
        private set
        {
            Transform.WorldPosition = value;
            UpdateColliderCaches();
        }
    }

    public void MoveByAmount(Vector2 amount)
    {
        Transform.TranslateLocal(amount);
        UpdateColliderCaches();
    }

    public void MoveToExactPosition(Vector2 position)
    {
        Transform.WorldPosition = position;
        UpdateColliderCaches();
    }

    public void RotateByAmount(float amount)
    {
        Transform.Rotate(amount);
        UpdateColliderCaches();
    }

    public override void Start()
    {
        var collider = Entity.Components.Get<Collider>();
        if (collider == null) return;
        Body.Inertia = collider.CalculateRotationalInertia(Body.Mass);
    }

    public bool TryCreate()
    {
        return this.ValidateMaxDensity() && this.ValidateMinDensity();
    }

    public void RotateToExactAngle(float angle)
    {
        Transform.WorldRotation = angle;
        UpdateColliderCaches();
    }

    public void Simulate(float time)
    {
        if (Body.IsStatic) return;

        time /= PhysicsSetting.Iterations;

        var acceleration = force / Body.Mass;
        if (Body.HasGravity) acceleration += PhysicsSetting.GravityDirection;

        Body.LinearVelocity += acceleration * time;

        Position += Body.LinearVelocity * time;
        Rotation += Body.AngularVelocity * time;
        force = Vector2.Zero;

        UpdateColliderCaches();
    }

    public void AddForce(Vector2 force)
    {
        this.force = force;
    }

    public override bool IsAbleToDuplicate()
    {
        return false;
    }

    public AABBCollision GetAABB()
    {
        return Entity.Components.Get<Collider>().GetAABB();
    }

    private void UpdateColliderCaches()
    {
        var collider = Entity.Components.Get<Collider>();
        collider?.InvalidateCaches();
    }
}