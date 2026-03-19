using Physics_Engine.Core.Collision;

namespace Physics_Engine.Core.Rigidbody
{
    public class CircleRigidbody2D : Rigidbody2D
    {
        private CircleArea CircleArea { get; set; }

        public override bool TryCreate()
        {
            CircleArea = Body.ShapeArea as CircleArea;
            this.ValidateMinSize();
            this.ValidateMaxSize();
            this.ValidateMinDensity();
            this.ValidateMaxDensity();
            return true;
        }

        public override AABBCollision GetAABB()
        {
            if (!Body.IsAABBCollisionUpdateRequired)
            {
                return Body.AABBCollision;
            }
            var minX = Position.x - CircleArea.Radius;
            var minY = Position.y - CircleArea.Radius;
            var maxX = Position.x + CircleArea.Radius;
            var maxY = Position.y + CircleArea.Radius;
            
            Body.IsAABBCollisionUpdateRequired = false;
            
            Body.AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
            return Body.AABBCollision;
        }
    }
}