using System;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Physics_2D;

namespace Physics_Engine.Core.Rigidbody
{
    public static class Rigidbody2DExtensions
    {
        public static bool ValidateMaxDensity(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.Mass > PhysicsSetting.MaxMass)) return true;
            var errorMessage = $"Area is Too Large";
            Logger.LogError(errorMessage);
            return false;
        }
        public static bool ValidateMinDensity(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.Mass < PhysicsSetting.MinMass)) return true;
            var errorMessage = $"Density is Too Small";
            Logger.LogError(errorMessage);
            return false;
        }
    }
}