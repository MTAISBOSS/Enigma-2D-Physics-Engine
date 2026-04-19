using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;

namespace Enigma_Framework.Core.Collision;

public  class ColliderBuilder
{
    public class Builder<T> where T : Collider, new()
    {
        private readonly T instance = new T();
        
        public Builder<T> WithTriggerState(bool isTrigger)
        {
            instance.IsTrigger = isTrigger;
            return this;
        }
        public Builder<T> WithPhysicsMaterial(PhysicMaterial material)
        {
            instance.Material = material;
            return this;
        }
        public Builder<T> WithShapeArea(ShapeArea area)
        {
            instance.ShapeArea = area;
            return this;
        }

        public Collider Build()
        {
            return instance;
        }
    }
}