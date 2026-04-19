using Enigma_Framework.Core.DependencyInjection;
using Enigma_Framework.Core.Interfaces;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Core.Transform;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public class PolygonCollider : Collider, IVertices, IIndices
{
    public BoxArea BoxArea { get; set; }
    public int[] Indices { get; set; }
    public Vector2[] Vertices { get; set; }
    public Vector2[] TransformedVertices { get; set; }

    public int[] CreateIndices()
    {
        var indices = new int[6];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;
        return indices;
    }

    public Vector2[] CreateVertices()
    {
        var left = -BoxArea.Width / 2f;
        var right = left + BoxArea.Width;
        var bottom = -BoxArea.Height / 2f;
        var top = bottom + BoxArea.Height;

        var vertices = new Vector2[4];
        vertices[0] = new Vector2(left, top);
        vertices[1] = new Vector2(right, top);
        vertices[2] = new Vector2(right, bottom);
        vertices[3] = new Vector2(left, bottom);

        return vertices;
    }

    public Vector2[] GetTransformedVertices()
    {
        if (IsTransformUpdateRequired)
        {
            var pose2D = new Pose2D(Position, Rotation);
            for (var i = 0; i < Vertices.Length; i++) TransformedVertices[i] = Vector2.Translate(Vertices[i], pose2D);
            IsTransformUpdateRequired = false;
        }

        return TransformedVertices;
    }

    public override void Start()
    {
        BoxArea = new BoxArea(Entity.Transform.WorldScale.x, Entity.Transform.WorldScale.y);
        Vertices = CreateVertices();
        Indices = CreateIndices();
        IsTransformUpdateRequired = true;
        TransformedVertices = new Vector2[Vertices.Length];
        PhysicsContext.RegisterCollider(this);
    }

    ~PolygonCollider()
    {
        PhysicsContext.UnregisterCollider(this);
    }

    public override bool Intersects(Collider other, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();
        return CollisionDetector.Intersect(this, other, out collisionInfo);
    }

    public override AABBCollision GetAABB()
    {
        if (!IsAabbCollisionUpdateRequired) return AABBCollision;

        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        var vertices = GetTransformedVertices();
        for (var i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            if (v.x < minX) minX = v.x;
            if (v.y < minY) minY = v.y;
            if (v.x > maxX) maxX = v.x;
            if (v.y > maxY) maxY = v.y;
        }

        IsAabbCollisionUpdateRequired = false;
        AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
        return AABBCollision;
    }

    public override float CalculateRotationalInertia(float mass)
    {
        return 1f / 12f * mass *
               (BoxArea.Width * BoxArea.Width + BoxArea.Height * BoxArea.Height);
    }
}