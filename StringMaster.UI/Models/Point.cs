using System;
using System.Diagnostics;

namespace StringMaster.UI.Models;

[DebuggerDisplay("{X}"+ "," + "{Y}" + "," + "{Z}")]
public sealed class Point : ICloneable, IEquatable<Point>
{
    private const double EPSILON = 1e-6;

    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public static Point Origin => new(0, 0, 0);

    public Point(double x, double y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    public Point(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool IsValid()
    {
        if (Math.Abs(X) < 1E+20 && Math.Abs(Y) < 1E+20 && Math.Abs(Z) < 1E+20)
            return X + Y + Z != 0.0;
        return false;
    }

    public override string ToString()
    {
        return $"X:{X},Y:{Y},Z:{Z}";
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    // Value semantics: equality by coordinates within a tolerance.
    public bool Equals(Point other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Math.Abs(X - other.X) <= EPSILON
            && Math.Abs(Y - other.Y) <= EPSILON
            && Math.Abs(Z - other.Z) <= EPSILON;
    }

    public override bool Equals(object obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        // Quantize coordinates to the tolerance so hash code is consistent with equality.
        unchecked
        {
            long xBits = BitConverter.DoubleToInt64Bits(Math.Round(X, 6));
            long yBits = BitConverter.DoubleToInt64Bits(Math.Round(Y, 6));
            long zBits = BitConverter.DoubleToInt64Bits(Math.Round(Z, 6));

            int hash = (int)(xBits ^ (xBits >> 32));
            hash = (hash * 397) ^ (int)(yBits ^ (yBits >> 32));
            hash = (hash * 397) ^ (int)(zBits ^ (zBits >> 32));
            return hash;
        }
    }
}
