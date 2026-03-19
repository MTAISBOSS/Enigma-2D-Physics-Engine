using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision
{
    public class CircleCollider : Collider
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }
        public override bool Intersects(Collider other, out CollisionInfo collisionInfo)
        {
            collisionInfo = new CollisionInfo();
            return CollisionDetector.Intersect(this, other, out collisionInfo);
        }
    }
}