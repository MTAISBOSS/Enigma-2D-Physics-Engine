using Physics_Engine.Core.Rigidbody;
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
            return CollisionDetector.Intersect(Owner.Components.Get<Rigidbody2D>(), other.Owner.Components.Get<Rigidbody2D>(), out collisionInfo);
        }
    }
}