using System;
using Physics_Engine.Math;

namespace Physics_Engine.Core
{
    public enum ShapeType : byte
    {
        Circle,
        Rectangle,
        Polygon
    }

    public class Rigidbody2D
    {
        private Vector2 linearVelocity;
        private Vector2 position;
        private Vector2 angularVelocity;
        private Vector2 rotation;

        public float mass;
        public float density;
        public float bounciness;
        public float area;
        public bool isStatic;
        public float radius;
        public float width;
        public float height;

        public ShapeType ShapeType;

        public Rigidbody2D(Vector2 position,
            float mass, float density, float bounciness, float area, bool isStatic, float radius, float width,
            float height, ShapeType shapeType)
        {
            this.mass = mass;
            this.density = density;
            this.bounciness = bounciness;
            this.area = area;
            this.isStatic = isStatic;
            this.radius = radius;
            this.width = width;
            this.height = height;
            ShapeType = shapeType;
            this.linearVelocity  = Vector2.Zero;
            this.position = position;
            this.angularVelocity = Vector2.Zero;
            this.rotation = Vector2.Zero;
        }

        public static bool CreateCircleBody(float radius, Vector2 position, float density, bool isStatic,
            float restitution, out Rigidbody2D body, out string errorMessage)
        {
            body = null;
            errorMessage = String.Empty;
            float area = radius * radius * (float)System.Math.PI;
            if (area < World.MinBodySize)
            {
                errorMessage = $"Area is Too Small";
                return false;
            }

            if (area > World.MaxBodySize)
            {
                errorMessage = $"Area is Too Large";
                return false;
            }

            if (density < World.MinBodySize)
            {
                errorMessage = $"Density is Too Small";
                return false;
            }

            if (density > World.MaxDensity)
            {
                errorMessage = $"Area is Too Large";
                return false;
            }

            restitution = Mathematics.Clamp(restitution, 0, 1);
            float mass = area * density;
            body = new Rigidbody2D(position, mass, density, restitution, area, isStatic, radius, 0, 0,
                ShapeType.Circle);
            return true;
        }
        public static bool CreateBoxBody(float width, float height,Vector2 position, float density, bool isStatic,
            float restitution, out Rigidbody2D body, out string errorMessage)
        {
            body = null;
            errorMessage = String.Empty;
            float area = width * height;
            if (area < World.MinBodySize)
            {
                errorMessage = $"Area is Too Small";
                return false;
            }

            if (area > World.MaxBodySize)
            {
                errorMessage = $"Area is Too Large";
                return false;
            }

            if (density < World.MinBodySize)
            {
                errorMessage = $"Density is Too Small";
                return false;
            }

            if (density > World.MaxDensity)
            {
                errorMessage = $"Area is Too Large";
                return false;
            }

            restitution = Mathematics.Clamp(restitution, 0, 1);
            float mass = area * density;
            body = new Rigidbody2D(position, mass, density, restitution, area, isStatic, 0, width, height,
                ShapeType.Rectangle);
            return true;
        }
    }
}