using System;

namespace ImageEdit.Maths
{
    public struct Vector2
    {
        public float X, Y;

        public float LengthSquared => X * X + Y * Y;
        public float Length => (float)Math.Sqrt(LengthSquared);

        public Vector2 Normalized => this * (1f / Length);

        public Vector2 Xx => new Vector2(X, X);
        public Vector2 Xy => new Vector2(X, Y);
        public Vector2 Yx => new Vector2(Y, X);
        public Vector2 Yy => new Vector2(Y, Y);

        public Vector2(float value)
        {
            X = value;
            Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 other)
        {
            X = other.X;
            Y = other.Y;
        }

        public Vector2(Vector3 vector) : this(vector.X, vector.Y) { }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
        {
            return blend * (b - a) + a;
        }

        public static float Dot(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return a.LengthSquared < b.LengthSquared ? a : b;
        }

        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return a.LengthSquared < b.LengthSquared ? b : a;
        }

        public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
        {
            vector.X = vector.X < min.X ? min.X : (vector.X > max.X ? max.X : vector.X);
            vector.Y = vector.Y < min.Y ? min.Y : (vector.Y > max.Y ? max.Y : vector.Y);
            return vector;
        }

        public static Vector2 CompMin(Vector2 a, Vector2 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            return a;
        }

        public static Vector2 CompMax(Vector2 a, Vector2 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            return a;
        }

        public static Vector2 operator +(Vector2 left, float right)
        {
            left.X += right;
            left.Y += right;
            return left;
        }

        public static Vector2 operator -(Vector2 left, float right)
        {
            left.X -= right;
            left.Y -= right;
            return left;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        public static Vector2 operator *(float scale, Vector2 vector)
        {
            vector.X *= scale;
            vector.Y *= scale;
            return vector;
        }

        public static Vector2 operator *(Vector2 vector, float scale)
        {
            vector.X *= scale;
            vector.Y *= scale;
            return vector;
        }

        public static Vector2 operator /(Vector2 vector, float scale)
        {
            vector.X /= scale;
            vector.Y /= scale;
            return vector;
        }
    }
}
