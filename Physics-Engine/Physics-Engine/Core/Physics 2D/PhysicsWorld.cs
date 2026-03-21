using System.Collections.Generic;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Core.Service_Locator;
using Physics_Engine.Graphics;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Physics_2D
{
    public class PhysicsWorld : IService
    {
        private readonly List<Rigidbody2D> _rigidbody2Ds = new List<Rigidbody2D>();
        private readonly List<CollisionManifold> _contacts = new List<CollisionManifold>();
        private Rigidbody2D _bodyA;
        private Rigidbody2D _bodyB;
        public readonly List<Vector2> ContactPoints = new List<Vector2>();
        public int BodyCount => _rigidbody2Ds.Count;
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
            for (int i = 0; i < PhysicsSetting.Iterations; i++)
            {
                SimulateMovement(time);
                SimulateCollision();
            }
            RemoveOutOfSightBodies();
        }

        private void RemoveOutOfSightBodies()
        {
            for (int i = 0; i < _rigidbody2Ds.Count; i++)
            {
                if (_rigidbody2Ds[i].GetAABB().Max.y < ShapeRenderer.Instance.MainCamera.Bottom)
                {
                    UnregisterRigidbody(_rigidbody2Ds[i]);
                }
            }
        }

        private void SimulateCollision()
        {
            _contacts.Clear();
            ContactPoints.Clear();
            for (int i = 0; i < _rigidbody2Ds.Count - 1; i++)
            {
                _bodyA = _rigidbody2Ds[i];
                for (int j = i + 1; j < _rigidbody2Ds.Count; j++)
                {
                    _bodyB = _rigidbody2Ds[j];
                    if (CollisionDetector.Intersect(_bodyA, _bodyB, out CollisionInfo info))
                    {
                        if (_bodyA.Body.IsStatic && _bodyB.Body.IsStatic)
                        {
                            continue;
                        }

                        if (_bodyA.Body.IsStatic)
                        {
                            _bodyB.MoveByAmount(info.Normal * info.Depth);
                        }
                        else if (_bodyB.Body.IsStatic)
                        {
                            _bodyA.MoveByAmount(-info.Normal * info.Depth);
                        }
                        else
                        {
                            _bodyA.MoveByAmount(-info.Normal * info.Depth / 2f);
                            _bodyB.MoveByAmount(info.Normal * info.Depth / 2f);
                        }

                        ContactDetector.FindContactPoints(_bodyA,_bodyB,out Vector2 contact1,out Vector2 contact2,out int contactCount);
                        CollisionManifold contact =
                            new CollisionManifold(_bodyA, _bodyB, info, contact1, contact2, contactCount);
                        _contacts.Add(contact);

                    }
                }
            }

            for (int i = 0; i < _contacts.Count; i++)
            {
                CollisionManifold contact = _contacts[i];
                CollisionResolver.Resolve(in contact);
                if (contact.ContactCount>0)
                {
                    ContactPoints.Add(contact.Contact1);
                    if (contact.ContactCount > 1)
                    {
                        ContactPoints.Add(contact.Contact2);
                    }
                }
            }
        }

        private void SimulateMovement(float time)
        {
            foreach (var rigidbody in _rigidbody2Ds)
            {
                rigidbody.Simulate(time);
            }
        }
    }
}