using Physics_Engine.Core.Collision;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class BoxRigidbody2D : Rigidbody2D, IVertices, IIndices
    {
        public BoxArea BoxArea { get; set; }

        public override bool TryCreate()
        {
            BoxArea = Body.ShapeArea as BoxArea;

            this.ValidateMinSize();
            this.ValidateMaxSize();
            this.ValidateMinDensity();
            this.ValidateMaxDensity();

            Body.Vertices = CreateVertices();
            Body.Indices = CreateIndices();
            Body.IsTransformUpdateRequired = true;
            Body.TransformedVertices = new Vector2[Body.Vertices.Length];
            Body.Inertia = CalculateRotationalInertia();
            return true;
        }

        public override AABBCollision GetAABB()
        {
            if (!Body.IsAabbCollisionUpdateRequired)
            {
                return Body.AABBCollision;
            }

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            Vector2[] vertices = GetTransformedVertices();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 v = vertices[i];
                if (v.x < minX)
                {
                    minX = v.x;
                }

                if (v.y < minY)
                {
                    minY = v.y;
                }

                if (v.x > maxX)
                {
                    maxX = v.x;
                }

                if (v.y > maxY)
                {
                    maxY = v.y;
                }
            }

            Body.IsAabbCollisionUpdateRequired = false;

            Body.AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
            return Body.AABBCollision;
        }

        public override float CalculateRotationalInertia()
        {
            return 0.08f * Body.Mass * (BoxArea.Width * BoxArea.Width + BoxArea.Height * BoxArea.Height) * 0.001f;
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
            if (Body.IsTransformUpdateRequired)
            {
                Transform.Pose2D pose2D = new Transform.Pose2D(Position, Rotation);
                for (int i = 0; i < Body.Vertices.Length; i++)
                {
                    Vector2 vertex = Body.Vertices[i];
                    Body.TransformedVertices[i] = Vector2.Translate(vertex, pose2D);
                }
            }

            Body.IsTransformUpdateRequired = false;
            return Body.TransformedVertices;
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