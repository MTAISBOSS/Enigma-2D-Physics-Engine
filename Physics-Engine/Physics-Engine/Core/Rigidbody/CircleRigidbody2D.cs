using Physics_Engine.Core.Collision;

namespace Physics_Engine.Core.Rigidbody
{
    public class CircleRigidbody2D : Rigidbody2D
    {
        public CircleArea CircleArea { get; set; }

        public override bool TryCreate()
        {
            CircleArea = Body.ShapeArea as CircleArea;
            this.ValidateMinSize();
            this.ValidateMaxSize();
            this.ValidateMinDensity();
            this.ValidateMaxDensity();
            
            Body.Inertia = CalculateRotationalInertia();
            return true;
        }

        public override AABBCollision GetAABB()
        {
            if (!Body.IsAabbCollisionUpdateRequired)
            {
                return Body.AABBCollision;
            }
            var minX = Position.x - CircleArea.Radius;
            var minY = Position.y - CircleArea.Radius;
            var maxX = Position.x + CircleArea.Radius;
            var maxY = Position.y + CircleArea.Radius;
            
            Body.IsAabbCollisionUpdateRequired = false;
            
            Body.AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
            return Body.AABBCollision;
        }

        public override float CalculateRotationalInertia()
        {
            return 0.5f * Body.Mass * CircleArea.Radius * CircleArea.Radius;
        }
    }
}