using System;
using Physics_Engine.Core;
using Physics_Engine.Core.Transform;

namespace Physics_Engine.Math
{
    public struct Vector2
    {
        public float x;
        public float y;

        public static Vector2 Zero = new(0, 0);
        public static Vector2 Right = new(1, 0);
        public static Vector2 Left = new(-1, 0);
        public static Vector2 Up = new(0, 1);
        public static Vector2 Down = new(0, -1);
        public static Vector2 One = new(1, 1);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(a.x * scalar, a.y * scalar);
        public static Vector2 operator *(float scalar,Vector2 a) => new Vector2(a.x * scalar, a.y * scalar);
        public static Vector2 operator /(Vector2 a, float scalar)
        {
            if (scalar == 0)
            {
                Console.Beep();
                throw new Exception("Can't Divide By Zero!");
            }

            return new Vector2(a.x / scalar, a.y / scalar);
        }
        public static Vector2 operator -(Vector2 a) => new Vector2(-1 * a.x, -1 * a.y);

        public static Vector2 Translate(Vector2 vector2, Transform transform)
        {
            //Rotation matrix
            return new Vector2(
                transform.Cos * vector2.x - transform.Sin * vector2.y + transform.PositionX,
                transform.Sin * vector2.x + transform.Cos * vector2.y + transform.PositionY
            );
        }

        private bool Equals(Vector2 other)
        {
            if (System.Math.Abs(this.x - other.x) < 0.0001f && System.Math.Abs(y - other.y) < 0.0001f)
            {
                return true;
            }
            
            return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector2 other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return new { x, y }.GetHashCode();
        }

        public override string ToString()
        {
            return $"x: {x}, y: {y}";
        }
    }
}