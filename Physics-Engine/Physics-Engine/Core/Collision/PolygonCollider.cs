using Physics_Engine.Core.Interfaces;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision
{
    public class PolygonCollider : Collider,IVertices, IIndices
    {
        public BoxArea BoxArea { get; set; }
        public int[] Indices { get; set; }
        public Vector2[] Vertices { get; set; }
        public Vector2[] TransformedVertices { get; set; }
        public override void Start()
        {
            BoxArea = new BoxArea(Entity.Transform.Scale.x, Entity.Transform.Scale.y);
            Vertices = CreateVertices();
            Indices = CreateIndices();
            IsTransformUpdateRequired = true;
            TransformedVertices = new Vector2[Vertices.Length];
           Logger.LogError($"PolygonCollider created for {Entity.Name} at position {Position}");
        }

        public override bool Intersects(Collider other, out CollisionInfo collisionInfo)
        {
            collisionInfo = new CollisionInfo();
            return CollisionDetector.Intersect(this, other, out collisionInfo);
        }
        public override AABBCollision GetAABB()
        {
            if (!IsAabbCollisionUpdateRequired)
            {
                return AABBCollision;
            }

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            Vector2[] vertices = GetTransformedVertices();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 v = vertices[i];
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
            return (1f/12f) * mass * 
                   (BoxArea.Width * BoxArea.Width + BoxArea.Height * BoxArea.Height);
        }
        public Vector2[] CreateVertices()
        {
            float left = -BoxArea.Width / 2f;
            float right = left + BoxArea.Width;
            float bottom = -BoxArea.Height / 2f;
            float top = bottom + BoxArea.Height;

            Vector2[] vertices = new Vector2[4];
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
                Transform.Pose2D pose2D = new Transform.Pose2D(Position, Rotation);
                for (int i = 0; i < Vertices.Length; i++)
                {
                    TransformedVertices[i] = Vector2.Translate(Vertices[i], pose2D);
                }
                IsTransformUpdateRequired = false;
            }

            return TransformedVertices;
        }

        public int[] CreateIndices()
        {
            int[] indices = new int[6];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;
            return indices;
        }
    }
}
