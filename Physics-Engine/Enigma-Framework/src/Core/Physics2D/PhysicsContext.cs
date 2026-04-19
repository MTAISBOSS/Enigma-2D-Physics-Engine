using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.DependencyInjection;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Graphics;
using Enigma_Framework.Math;
using Enigma_Framework.Utilities;

namespace Enigma_Framework.Core.Physics2D;

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

public static class PhysicsContext
{
    private static readonly List<Collider> Colliders = new(100);
    private static readonly List<ContactPair> ContactPairs = new(100);
    private static readonly List<CollisionManifold> Manifolds = new(100);
    private static readonly SpatialHashGrid Grid = new(40);
    private static readonly HashSet<long> PairTracker = new();
    private static readonly List<Rigidbody2D> Rigidbody2Ds = new(100);
    private static AABBCollision[] _cachedAabBs = new AABBCollision[100];
    public static int BodyCount => Rigidbody2Ds.Count;
    public static List<Collider> GetColliders() => Colliders;

    public static void Initialize()
    {
        
    }
    public static void RegisterRigidbody(Rigidbody2D rigidbody2D)
    {
        if (!Rigidbody2Ds.Contains(rigidbody2D))
            Rigidbody2Ds.Add(rigidbody2D);
    }

    public static void UnregisterRigidbody(Rigidbody2D rigidbody2D)
    {
        Rigidbody2Ds.Remove(rigidbody2D);
    }

    public static void RegisterCollider(Collider collider)
    {
        if (!Colliders.Contains(collider))
            Colliders.Add(collider);
    }

    public static void UnregisterCollider(Collider collider)
    {
        Colliders.Remove(collider);
    }

    public static void Simulate(float time)
    {
        // 1. Integrate movement once
        SimulateMovement(time);

        ContactPairs.Clear();
        Manifolds.Clear();
        PairTracker.Clear();
        Grid.Clear();

        // 2. Detect collisions once
        BroadPhase();
        NarrowPhase();

        // 3. Iterative impulse solver
        for (int i = 0; i < PhysicsSetting.Iterations; i++)
        {
            foreach (var manifold in Manifolds)
            {
                CollisionResolver.ResolveWithRotationWithFriction(in manifold);
            }
        }

        // 4. Positional correction once
        foreach (var manifold in Manifolds)
        {
            var bodyA = manifold.BodyA;
            var bodyB = manifold.BodyB;

            SeparateBodies(bodyA, bodyB, in manifold.CollisionInfo);
        }

        RemoveOutOfSightBodies();
    }

    private static void SimulateMovement(float time)
    {
        foreach (var rigidbody2D in Rigidbody2Ds)
            rigidbody2D.Simulate(time);
    }

    private static void BroadPhase()
    {
        var count = Rigidbody2Ds.Count;

        if (_cachedAabBs.Length < count)
            Array.Resize(ref _cachedAabBs, count * 2);

        for (var i = 0; i < count; i++)
        {
            _cachedAabBs[i] = Rigidbody2Ds[i].GetAABB();
            Grid.Insert(i, _cachedAabBs[i]);
        }

        foreach (var cell in Grid.GetActiveCells())
        {
            for (var i = 0; i < cell.Count - 1; i++)
            {
                var indexA = cell[i];
                var bodyA = Rigidbody2Ds[indexA];
                var aabbA = _cachedAabBs[indexA];

                for (var j = i + 1; j < cell.Count; j++)
                {
                    var indexB = cell[j];

                    var min = System.Math.Min(indexA, indexB);
                    var max = System.Math.Max(indexA, indexB);
                    var pairId = ((long)min << 32) | (uint)max;

                    if (!PairTracker.Add(pairId))
                        continue;

                    var bodyB = Rigidbody2Ds[indexB];

                    if (bodyA.Body.IsStatic && bodyB.Body.IsStatic)
                        continue;

                    if (!CollisionDetector.IntersectAABBs(aabbA, _cachedAabBs[indexB]))
                        continue;

                    ContactPairs.Add(new ContactPair(min, max));
                }
            }
        }
    }

    private static void NarrowPhase()
    {
        foreach (var pair in ContactPairs)
        {
            var bodyA = Rigidbody2Ds[pair.Item1];
            var bodyB = Rigidbody2Ds[pair.Item2];

            var colA = bodyA.Entity.Components.Get<Collider>();
            var colB = bodyB.Entity.Components.Get<Collider>();

            if (!CollisionDetector.Intersect(colA, colB, out var info))
                continue;

            if (!colA.IsTrigger && !colB.IsTrigger)
            {
                ContactDetector.FindContactPoints(
                    colA,
                    colB,
                    out var contact1,
                    out var contact2,
                    out var contactCount
                );

                var manifold = new CollisionManifold(
                    colA,
                    colB,
                    info,
                    contact1,
                    contact2,
                    contactCount
                );

                Manifolds.Add(manifold);
            }

            if (colA.IsTrigger && !colB.IsTrigger)
                colA.OnTriggerEnter(colB);

            if (!colA.IsTrigger && colB.IsTrigger)
                colB.OnTriggerEnter(colA);
        }
    }

    private static void SeparateBodies(Rigidbody2D bodyA, Rigidbody2D bodyB, in CollisionInfo info)
    {
        const float percent = 0.8f;
        const float slop = 0.01f;

        float correctionMagnitude =
            System.Math.Max(info.Depth - slop, 0.0f) /
            (bodyA.Body.InverseMass + bodyB.Body.InverseMass) *
            percent;

        Vector2 correction = info.Normal * correctionMagnitude;

        if (!bodyA.Body.IsStatic)
            bodyA.MoveByAmount(-correction * bodyA.Body.InverseMass);

        if (!bodyB.Body.IsStatic)
            bodyB.MoveByAmount(correction * bodyB.Body.InverseMass);
    }

    private static void RemoveOutOfSightBodies()
    {
        for (var i = Rigidbody2Ds.Count - 1; i >= 0; i--)
        {
            if (!(Rigidbody2Ds[i].GetAABB().Max.y < ShapeRenderer.Instance.MainCamera.Bottom))
                continue;

            Rigidbody2Ds[i] = Rigidbody2Ds[Rigidbody2Ds.Count - 1];
            Rigidbody2Ds.RemoveAt(Rigidbody2Ds.Count - 1);
        }
    }


    
}
