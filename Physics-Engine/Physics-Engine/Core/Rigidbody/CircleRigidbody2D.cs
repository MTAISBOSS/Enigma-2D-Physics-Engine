namespace Physics_Engine.Core.Rigidbody
{
    public class CircleRigidbody2D : Rigidbody2D
    {
        private CircleShapeArea CircleShapeArea { get; set; }

        protected override bool TryCreate()
        {
            CircleShapeArea = Body.ShapeArea as CircleShapeArea;
            this.ValidateMinSize();
            this.ValidateMaxSize();
            this.ValidateMinDensity();
            this.ValidateMaxDensity();
            return true;
        }
    }
}