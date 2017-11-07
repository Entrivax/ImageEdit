using System;

namespace ImageEdit.Maths
{
    public struct Vector4
    {
        public float X, Y, Z, W;

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;
        public float Length => (float)Math.Sqrt(LengthSquared);

        public Vector4 Normalized => this * (1f / Length);

        public Vector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector2 vector) : this(vector.X, vector.Y, 0, 0) { }
        public Vector4(Vector2 vector, float z, float w) : this(vector.X, vector.Y, z, w) { }
        public Vector4(Vector3 vector) : this(vector.X, vector.Y, vector.Z, 0) { }
        public Vector4(Vector3 vector, float w) : this(vector.X, vector.Y, vector.Z, w) { }
        
        public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
        {
            return blend * (b - a) + a;
        }

        public static float Dot(Vector4 left, Vector4 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        public static Vector4 Min(Vector4 a, Vector4 b)
        {
            return a.LengthSquared < b.LengthSquared ? a : b;
        }

        public static Vector4 Max(Vector4 a, Vector4 b)
        {
            return a.LengthSquared < b.LengthSquared ? b : a;
        }

        public static Vector4 Clamp(Vector4 vector, Vector4 min, Vector4 max)
        {
            vector.X = vector.X < min.X ? min.X : (vector.X > max.X ? max.X : vector.X);
            vector.Y = vector.Y < min.Y ? min.Y : (vector.Y > max.Y ? max.Y : vector.Y);
            vector.Z = vector.Z < min.Z ? min.Z : (vector.Z > max.Z ? max.Z : vector.Z);
            vector.W = vector.W < min.W ? min.W : (vector.W > max.W ? max.W : vector.W);
            return vector;
        }

        public static Vector4 CompMin(Vector4 a, Vector4 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        public static Vector4 CompMax(Vector4 a, Vector4 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        public static Vector4 operator +(Vector4 left, float right)
        {
            left.X += right;
            left.Y += right;
            left.Z += right;
            left.W += right;
            return left;
        }

        public static Vector4 operator -(Vector4 left, float right)
        {
            left.X -= right;
            left.Y -= right;
            left.Z -= right;
            left.W -= right;
            return left;
        }

        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        public static Vector4 operator *(Vector4 left, Vector4 right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
            return left;
        }

        public static Vector4 operator *(float scale, Vector4 vector)
        {
            vector.X *= scale;
            vector.Y *= scale;
            vector.Z *= scale;
            vector.W *= scale;
            return vector;
        }

        public static Vector4 operator *(Vector4 vector, float scale)
        {
            vector.X *= scale;
            vector.Y *= scale;
            vector.Z *= scale;
            vector.W *= scale;
            return vector;
        }

        public static Vector4 operator /(Vector4 vector, float scale)
        {
            vector.X /= scale;
            vector.Y /= scale;
            vector.Z /= scale;
            vector.W /= scale;
            return vector;
        }

        public static Vector4 operator /(Vector4 left, Vector4 right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            left.W /= right.W;
            return left;
        }
    }
}
