using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision
{
    public class CircleCollider : Collider
    {
        public CircleArea CircleArea { get; set; }
        public override bool Intersects(Collider other, out CollisionInfo collisionInfo)
        {
            collisionInfo = new CollisionInfo();
            return CollisionDetector.Intersect(this, other, out collisionInfo);
        }
        public override void Start()
        {
            CircleArea = new CircleArea(Entity.Transform.Scale.x);
            IsTransformUpdateRequired = true;
            Logger.LogError($"CircleCollider created for {Entity.Name} at position {Position}");
        }
        public override AABBCollision GetAABB()
        {
            if (!IsAabbCollisionUpdateRequired)
            {
                return AABBCollision;
            }
            var minX = Position.x - CircleArea.Radius;
            var minY = Position.y - CircleArea.Radius;
            var maxX = Position.x + CircleArea.Radius;
            var maxY = Position.y + CircleArea.Radius;
            
            IsAabbCollisionUpdateRequired = false;
            
            AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
            return AABBCollision;
        }
        public override float CalculateRotationalInertia(float mass)
        {
            return 0.5f * mass * CircleArea.Radius * CircleArea.Radius;
        }
    }
}