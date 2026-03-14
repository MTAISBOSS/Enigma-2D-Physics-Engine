using Physics_Engine.Math;

namespace Physics_Engine.Core.Rigidbody
{
    public class BoxRigidbody2D : Rigidbody2D , IVertices, IIndices
    {
        public BoxShapeArea BoxShapeArea { get; set; }
        protected override bool TryCreate()
        {
            BoxShapeArea = Body.ShapeArea as BoxShapeArea;
            
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

        public Vector2[] CreateVertices()
        {

            float left = -BoxShapeArea.Width / 2f;
            float right = left + BoxShapeArea.Width;
            float bottom = -BoxShapeArea.Height / 2f;
            float top = bottom + BoxShapeArea.Height;

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
                Transform transform = new Transform(Body.Position,Body.Rotation);
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