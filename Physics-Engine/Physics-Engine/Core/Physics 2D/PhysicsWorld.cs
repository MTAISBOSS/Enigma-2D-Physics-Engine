using System.Collections.Generic;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Core.Service_Locator;

namespace Physics_Engine.Core.Physics_2D
{
    public class PhysicsWorld : IService
    {
        private readonly List<Rigidbody2D> _rigidbody2Ds = new List<Rigidbody2D>();
        private Collider _colliderA;
        private Collider _colliderB;

        public PhysicsWorld()
        {
            ServiceLocator.Instance.Register(this);
        }

        ~PhysicsWorld()
        {
            ServiceLocator.Instance.Unregister(this);
        }

        public void RegisterRigidbody(Rigidbody2D rigidbody2D)
        {
            if (!_rigidbody2Ds.Contains(rigidbody2D))
            {
                _rigidbody2Ds.Add(rigidbody2D);
            }
        }

        public void UnregisterRigidbody(Rigidbody2D rigidbody2D)
        {
            if (_rigidbody2Ds.Contains(rigidbody2D))
            {
                _rigidbody2Ds.Remove(rigidbody2D);
            }
        }

        public void Simulate(float time)
        {
            SimulateMovement(time);
            SimulateCollision();
        }

        private void SimulateCollision()
        {
            for (int i = 0; i < _rigidbody2Ds.Count - 1; i++)
            {
                _colliderA = _rigidbody2Ds[i].Owner.Components.Get<Collider>();
                for (int j = i + 1; j < _rigidbody2Ds.Count; j++)
                {
                    _colliderB = _rigidbody2Ds[j].Owner.Components.Get<Collider>();
                    if (CollisionDetector.Intersect(_colliderA, _colliderB, out CollisionInfo info))
                    {
                        Rigidbody2D bodyA = _colliderA.Owner.Components.Get<Rigidbody2D>();
                        Rigidbody2D bodyB = _colliderB.Owner.Components.Get<Rigidbody2D>();
                        if (bodyA.Body.IsStatic && bodyB.Body.IsStatic)
                        {
                            continue;
                        }

                        if (bodyA.Body.IsStatic)
                        {
                            bodyB.MoveByAmount(info.Normal * info.Depth);
                        }
                        else if (bodyB.Body.IsStatic)
                        {
                            bodyA.MoveByAmount(-info.Normal * info.Depth);
                        }
                        else
                        {
                            bodyA.MoveByAmount(-info.Normal * info.Depth / 2f);
                            bodyB.MoveByAmount(info.Normal * info.Depth / 2f);
                        }


                        CollisionResolver.Resolve(bodyA, bodyB, info);
                    }
                }
            }
        }

        private void SimulateMovement(float time)
        {
            for (int i = 0; i < _rigidbody2Ds.Count; i++)
            {
                _rigidbody2Ds[i].Simulate(time);
            }
        }
    }
}