using System;

namespace Engine.Geom
{
    /// <summary>
    /// The Point class describes a two dimensional point or vector.
    /// </summary>
    public class Point
    {

        public float X;
        public float Y;

        public Point(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
            set
            {
                X = X * value;
                Y = Y * value;
            }
        }

        public float Angle => (float)Math.Atan2(Y, X);

        public bool IsOrigin => X == 0.0f && Y == 0.0f;

        public void AddPoint(Point point)
        {
            X = X + point.X;
            Y = Y + point.Y;
        }

        public void SubtractPoint(Point point)
        {
            X = X - point.X;
            Y = Y - point.Y;
        }

        /// <summary>
        /// Rotates by the specified angle in Radians
        /// </summary>
        public void RotateBy(float angle)
        {
            float sin = MathUtil.FastSin(angle);
            float cos = MathUtil.FastCos(angle);
            X = X * cos - Y * sin;
            Y = X * sin + Y * cos;
        }

        public void Normalize()
        {
            if (IsOrigin)
            {
                return;
            }
            float inverseLength = 1 / Length;
            X = X * inverseLength;
            Y = Y * inverseLength;
        }

        /// <summary>
        /// Calculates the dot product of this Point and another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The dot product</returns>
        public float Dot(Point other)
        {
            return X * other.X + Y * other.Y;
        }

        public void CopyFromPoint(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void SetTo(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether the specified Point's X and Y values is equal to the current Point with
        /// with a small epsilon error margin.
        /// </summary>
        public bool Equals(Point other)
        {
            if (other == this)
            {
                return true;
            } 

            if (other == null)
            {
                return false;
            }

            return MathUtil.IsAlmostEqual(X, other.X) && MathUtil.IsAlmostEqual(Y, other.Y);
        }

        public float Distance(Point p2)
        {
            return (float)Math.Sqrt((X - p2.X) * (X - p2.X) + (Y - p2.Y) * (Y - p2.Y));
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}