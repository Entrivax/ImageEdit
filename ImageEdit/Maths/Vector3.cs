using System;

namespace ImageEdit.Maths
{
    public struct Vector3
    {
        public float X, Y, Z;

        public float LengthSquared => X * X + Y * Y + Z * Z;
        public float Length => (float)Math.Sqrt(LengthSquared);

        public Vector3 Normalized => this * (1f / Length);

        public Vector2 Xx => new Vector2(X, X);
        public Vector2 Xy => new Vector2(X, Y);
        public Vector2 Xz => new Vector2(X, Z);
        public Vector2 Yx => new Vector2(Y, X);
        public Vector2 Yy => new Vector2(Y, Y);
        public Vector2 Yz => new Vector2(Y, Z);
        public Vector2 Zx => new Vector2(Z, X);
        public Vector2 Zy => new Vector2(Z, Y);
        public Vector2 Zz => new Vector2(Z, Z);

        public Vector3 Xxx => new Vector3(X, X, X);
        public Vector3 Xxy => new Vector3(X, X, Y);
        public Vector3 Xxz => new Vector3(X, X, Z);
        public Vector3 Xyx => new Vector3(X, Y, X);
        public Vector3 Xyy => new Vector3(X, Y, Y);
        public Vector3 Xyz => new Vector3(X, Y, Z);
        public Vector3 Xzx => new Vector3(X, Z, X);
        public Vector3 Xzy => new Vector3(X, Z, Y);
        public Vector3 Xzz => new Vector3(X, Z, Z);

        public Vector3 Yxx => new Vector3(Y, X, X);
        public Vector3 Yxy => new Vector3(Y, X, Y);
        public Vector3 Yxz => new Vector3(Y, X, Z);
        public Vector3 Yyx => new Vector3(Y, Y, X);
        public Vector3 Yyy => new Vector3(Y, Y, Y);
        public Vector3 Yyz => new Vector3(Y, Y, Z);
        public Vector3 Yzx => new Vector3(Y, Z, X);
        public Vector3 Yzy => new Vector3(Y, Z, Y);
        public Vector3 Yzz => new Vector3(Y, Z, Z);

        public Vector3 Zxx => new Vector3(Z, X, X);
        public Vector3 Zxy => new Vector3(Z, X, Y);
        public Vector3 Zxz => new Vector3(Z, X, Z);
        public Vector3 Zyx => new Vector3(Z, Y, X);
        public Vector3 Zyy => new Vector3(Z, Y, Y);
        public Vector3 Zyz => new Vector3(Z, Y, Z);
        public Vector3 Zzx => new Vector3(Z, Z, X);
        public Vector3 Zzy => new Vector3(Z, Z, Y);
        public Vector3 Zzz => new Vector3(Z, Z, Z);

        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 vector) : this(vector, 0) { }

        public Vector3(Vector2 vector, float z)
        {
            X = vector.X;
            Y = vector.Y;
            Z = z;
        }

        public Vector3(float x, Vector2 vector)
        {
            X = x;
            Y = vector.X;
            Z = vector.Y;
        }

        public Vector3(Vector3 other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
        {
            return blend * (b - a) + a;
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return a.LengthSquared < b.LengthSquared ? a : b;
        }

        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return a.LengthSquared < b.LengthSquared ? b : a;
        }

        public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
        {
            vector.X = vector.X < min.X ? min.X : (vector.X > max.X ? max.X : vector.X);
            vector.Y = vector.Y < min.Y ? min.Y : (vector.Y > max.Y ? max.Y : vector.Y);
            vector.Z = vector.Z < min.Z ? min.Z : (vector.Z > max.Z ? max.Z : vector.Z);
            return vector;
        }

        public static Vector3 CompMin(Vector3 a, Vector3 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            return a;
        }

        public static Vector3 CompMax(Vector3 a, Vector3 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            return a;
        }

        public static Vector3 operator +(Vector3 left, float right)
        {
            left.X += right;
            left.Y += right;
            left.Z += right;
            return left;
        }

        public static Vector3 operator -(Vector3 left, float right)
        {
            left.X -= right;
            left.Y -= right;
            left.Z -= right;
            return left;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        public static Vector3 operator *(float scale, Vector3 vector)
        {
            vector.X *= scale;
            vector.Y *= scale;
            vector.Z *= scale;
            return vector;
        }

        public static Vector3 operator *(Vector3 vector, float scale)
        {
            vector.X *= scale;
            vector.Y *= scale;
            vector.Z *= scale;
            return vector;
        }

        public static Vector3 operator /(Vector3 vector, float scale)
        {
            vector.X /= scale;
            vector.Y /= scale;
            vector.Z /= scale;
            return vector;
        }
    }
}
