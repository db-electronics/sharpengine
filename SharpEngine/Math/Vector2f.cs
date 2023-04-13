using System.Security.Policy;

namespace SharpEngine.Math
{
    public class Vector2f
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector2f()
        {
            X = 0f;
            Y = 0f;
        }

        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2f(float a)
        {
            X = a;
            Y = a;
        }

        public Vector2f Swapped()
        {
            return new Vector2f(Y, X);
        }

        public void SwapXY()
        {
            (X, Y) = (Y, X);
        }

        public static Vector2f Zero()
        {
            return new Vector2f(0f, 0f);
        }

        public static Vector2f Unit()
        {
            return new Vector2f(1f, 1f);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static Vector2f operator *(Vector2f a, float b) => new(a.X * b, a.Y * b);
        public static Vector2f operator *(Vector2f a, Vector2f b) => new (a.X * b.X, a.Y * b.Y);
        public static Vector2f operator +(Vector2f a, float b) => new(a.X + b, a.Y + b);
        public static Vector2f operator +(Vector2f a, Vector2f b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2f operator -(Vector2f a, float b) => new(a.X - b, a.Y - b);
        public static Vector2f operator -(Vector2f a, Vector2f b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2f operator /(Vector2f a, float b) => new(a.X / b, a.Y / b);
    }
}