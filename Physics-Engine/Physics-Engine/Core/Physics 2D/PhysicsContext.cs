using System;
using System.Collections.Generic;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Core.Service_Locator;
using Physics_Engine.Graphics;
using Physics_Engine.Math;
using Physics_Engine.Utilities;

namespace Physics_Engine.Core.Physics_2D
{
    internal struct ContactPair
    {
        public readonly int Item1;
        public readonly int Item2;

        public ContactPair(int item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public class PhysicsContext : IService
    {
        private readonly List<Rigidbody2D> _rigidbody2Ds = new(1000);
        private readonly List<ContactPair> _contactPairs = new(1000);
        private AABBCollision[] _cachedAabBs = new AABBCollision[1000];

        private readonly SpatialHashGrid _grid = new SpatialHashGrid(200);

        private readonly HashSet<long> _pairTracker = new HashSet<long>();
        private int _currentIteration;

        public int BodyCount => _rigidbody2Ds.Count;

        public PhysicsContext()
        {
            ServiceLocator.Instance.Register(this);
        }

        ~PhysicsContext()
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
            _rigidbody2Ds.Remove(rigidbody2D);
        }

        public void Simulate(float time)
        {
            for (int currentIteration = 0; currentIteration < PhysicsSetting.Iterations; currentIteration++)
            {
                _currentIteration = currentIteration;
                SimulateMovement(time);
                SimulateCollision();
            }

            RemoveOutOfSightBodies();
        }

        private void SimulateMovement(float time)
        {
            for (int i = 0; i < _rigidbody2Ds.Count; i++)
            {
                _rigidbody2Ds[i].Simulate(time);
            }
        }

        private void SimulateCollision()
        {
            _contactPairs.Clear();
            _pairTracker.Clear();
            _grid.Clear();

            BroadPhase();
            NarrowPhase();
        }


        private void BroadPhase()
        {
            int count = _rigidbody2Ds.Count;

            if (_cachedAabBs.Length < count)
                Array.Resize(ref _cachedAabBs, count * 2);

            for (int i = 0; i < count; i++)
            {
                _cachedAabBs[i] = _rigidbody2Ds[i].GetAABB();
                _grid.Insert(i, _cachedAabBs[i]);
            }

            foreach (List<int> cell in _grid.GetActiveCells())
            {
                for (int i = 0; i < cell.Count - 1; i++)
                {
                    int indexA = cell[i];
                    Rigidbody2D bodyA = _rigidbody2Ds[indexA];
                    AABBCollision aabbA = _cachedAabBs[indexA];

                    for (int j = i + 1; j < cell.Count; j++)
                    {
                        int indexB = cell[j];

                        int min = System.Math.Min(indexA, indexB);
                        int max = System.Math.Max(indexA, indexB);
                        long pairId = ((long)min << 32) | (uint)max;

                        if (!_pairTracker.Add(pairId))
                            continue;

                        Rigidbody2D bodyB = _rigidbody2Ds[indexB];

                        if (bodyA.Body.IsStatic && bodyB.Body.IsStatic)
                            continue;

                        if (!CollisionDetector.IntersectAABBs(aabbA, _cachedAabBs[indexB]))
                            continue;

                        _contactPairs.Add(new ContactPair(min, max));
                    }
                }
            }
        }

        private void NarrowPhase()
        {
            for (int i = 0; i < _contactPairs.Count; i++)
            {
                var pair = _contactPairs[i];
                var bodyA = _rigidbody2Ds[pair.Item1];
                var bodyB = _rigidbody2Ds[pair.Item2];

                if (CollisionDetector.Intersect(bodyA, bodyB, out CollisionInfo info))
                {
                    SeparateBodies(bodyA, bodyB, in info);

                    ContactDetector.FindContactPoints(bodyA, bodyB, out Vector2 contact1, out Vector2 contact2,
                        out int contactCount);

                    CollisionManifold contact =
                        new CollisionManifold(bodyA, bodyB, info, contact1, contact2, contactCount);
                    CollisionResolver.ResolveWithRotationWithFriction(in contact);
                }
            }
        }

        private void SeparateBodies(Rigidbody2D bodyA, Rigidbody2D bodyB, in CollisionInfo info)
        {
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
        }
        private void RemoveOutOfSightBodies()
        {
            for (int i = _rigidbody2Ds.Count - 1; i >= 0; i--)
            {
                if (_rigidbody2Ds[i].GetAABB().Max.y < ShapeRenderer.Instance.MainCamera.Bottom)
                {
                    _rigidbody2Ds[i] = _rigidbody2Ds[_rigidbody2Ds.Count - 1];
                    _rigidbody2Ds.RemoveAt(_rigidbody2Ds.Count - 1);
                }
            }
        }
    }
}