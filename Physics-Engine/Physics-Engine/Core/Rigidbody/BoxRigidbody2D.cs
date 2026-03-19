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

            return true;
        }

        public override AABBCollision GetAABB()
        {
            if (!Body.IsAABBCollisionUpdateRequired)
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
            
            Body.IsAABBCollisionUpdateRequired = false;

            Body.AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
            return Body.AABBCollision;
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
                Transform.Transform transform = new Transform.Transform(Position, Rotation);
                for (int i = 0; i < Body.Vertices.Length; i++)
                {
                    Vector2 vertex = Body.Vertices[i];
                    Body.TransformedVertices[i] = Vector2.Translate(vertex, transform);
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