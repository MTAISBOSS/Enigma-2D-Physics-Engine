using System;
using Physics_Engine.Core.Physics_2D;

namespace Physics_Engine.Core.Rigidbody
{
    public static class Rigidbody2DExtensions
    {
        public static bool ValidateMaxDensity(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.Density > Physics2DSetting.MaxDensity)) return true;
            var errorMessage = $"Area is Too Large";
            throw new Exception(errorMessage);

        }
        public static bool ValidateMinDensity(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.Density < Physics2DSetting.MinDensity)) return true;
            var errorMessage = $"Density is Too Small";
            throw new Exception(errorMessage);
        }
        public static bool ValidateMaxSize(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.ShapeArea.Calculate() > Physics2DSetting.MaxBodySize)) return true;
            var errorMessage = $"Area is Too Large";
            throw new Exception(errorMessage);

        }
        public static bool ValidateMinSize(this Rigidbody2D rigidbody)
        {
            if (!(rigidbody.Body.ShapeArea.Calculate() < Physics2DSetting.MinBodySize)) return true;
            var errorMessage = $"Area is Too Small";
            throw new Exception(errorMessage);

        }
    }
}