using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Rigidbody2D> rigidbody2Ds = new(100);
        private readonly List<Collider> colliders = new(100);
        private readonly List<ContactPair> contactPairs = new(100);
        private AABBCollision[] cachedAabBs = new AABBCollision[100];

        private readonly SpatialHashGrid grid = new SpatialHashGrid(40);

        private readonly HashSet<long> pairTracker = new HashSet<long>();
        private int currentIteration;

        public int BodyCount => rigidbody2Ds.Count;

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
            if (!rigidbody2Ds.Contains(rigidbody2D))
            {
                rigidbody2Ds.Add(rigidbody2D);
            }
        }

        public void UnregisterRigidbody(Rigidbody2D rigidbody2D)
        {
            rigidbody2Ds.Remove(rigidbody2D);
        } 
        public void RegisterCollider(Collider collider)
        {
            if (!colliders.Contains(collider))
            {
                colliders.Add(collider);
            }
        }

        public void UnregisterCollider(Collider collider)
        {
            colliders.Remove(collider);
        }

        public void Simulate(float time)
        {
            for (int iteration = 0; iteration < PhysicsSetting.Iterations; iteration++)
            {
                currentIteration = iteration;
                SimulateMovement(time);
                SimulateCollision();
            }

            RemoveOutOfSightBodies();
        }

        private void SimulateMovement(float time)
        {
            foreach (var rigidbody2D in rigidbody2Ds)
            {
                rigidbody2D.Simulate(time);
            }
        }

        private void SimulateCollision()
        {
            contactPairs.Clear();
            pairTracker.Clear();
            grid.Clear();

            BroadPhase();
            NarrowPhase();
        }


        private void BroadPhase()
        {
            int count = rigidbody2Ds.Count;

            if (cachedAabBs.Length < count)
                Array.Resize(ref cachedAabBs, count * 2);

            for (int i = 0; i < count; i++)
            {
                cachedAabBs[i] = rigidbody2Ds[i].GetAABB();
               // Console.WriteLine($"Body {i} ({rigidbody2Ds[i].Entity.Name}): AABB Min({cachedAabBs[i].Min.x}, {cachedAabBs[i].Min.y}) Max({cachedAabBs[i].Max.x}, {cachedAabBs[i].Max.y})");
                grid.Insert(i, cachedAabBs[i]);
            }

            foreach (List<int> cell in grid.GetActiveCells())
            {
                for (int i = 0; i < cell.Count - 1; i++)
                {
                    int indexA = cell[i];
                    Rigidbody2D bodyA = rigidbody2Ds[indexA];
                    AABBCollision aabbA = cachedAabBs[indexA];

                    for (int j = i + 1; j < cell.Count; j++)
                    {
                        int indexB = cell[j];

                        int min = System.Math.Min(indexA, indexB);
                        int max = System.Math.Max(indexA, indexB);
                        long pairId = ((long)min << 32) | (uint)max;

                        if (!pairTracker.Add(pairId))
                            continue;

                        Rigidbody2D bodyB = rigidbody2Ds[indexB];

                        if (bodyA.Body.IsStatic && bodyB.Body.IsStatic)
                            continue;

                        if (!CollisionDetector.IntersectAABBs(aabbA, cachedAabBs[indexB]))
                        {
                            // Add this debug line
                           // Console.WriteLine($"AABB A: Min({aabbA.Min.x}, {aabbA.Min.y}) Max({aabbA.Max.x}, {aabbA.Max.y})");
                            //Console.WriteLine($"AABB B: Min({cachedAabBs[indexB].Min.x}, {cachedAabBs[indexB].Min.y}) Max({cachedAabBs[indexB].Max.x}, {cachedAabBs[indexB].Max.y})");
                            continue;
                        }


                        contactPairs.Add(new ContactPair(min, max));
                    }
                }
            }
        }

        private void NarrowPhase()
        {
            foreach (var pair in contactPairs)
            {
                var bodyA = rigidbody2Ds[pair.Item1];
                var bodyB = rigidbody2Ds[pair.Item2];

                var colA = bodyA.Entity.Components.Get<Collider>();
                var colB = bodyB.Entity.Components.Get<Collider>();


                if (!CollisionDetector.Intersect(colA, colB, out CollisionInfo info)) continue;

                if (!colA.IsTrigger && !colB.IsTrigger)
                {
                    SeparateBodies(bodyA, bodyB, in info);
                    ContactDetector.FindContactPoints(colA, colB, out Vector2 contact1, out Vector2 contact2,
                        out int contactCount);
                    CollisionManifold contact =
                        new CollisionManifold(colA, colB, info, contact1, contact2, contactCount);
                    CollisionResolver.ResolveWithRotationWithFriction(in contact);
                }

                if (colA.IsTrigger && !colB.IsTrigger)
                {
                    colA.OnTriggerEnter(colB);
                }

                if (!colA.IsTrigger && colB.IsTrigger)
                {
                    colB.OnTriggerEnter(colA);
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
            for (int i = rigidbody2Ds.Count - 1; i >= 0; i--)
            {
                if (!(rigidbody2Ds[i].GetAABB().Max.y < ShapeRenderer.Instance.MainCamera.Bottom)) continue;
                rigidbody2Ds[i] = rigidbody2Ds[rigidbody2Ds.Count - 1];
                rigidbody2Ds.RemoveAt(rigidbody2Ds.Count - 1);
            }
        }

        public List<Collider> GetColliders()
        {
            return colliders;
        }
    }
}