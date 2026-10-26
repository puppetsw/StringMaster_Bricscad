using System;
using System.Collections.Generic;

namespace StringMaster.UI.Models;

public sealed class SurveyPoint : IEquatable<SurveyPoint>, ICloneable
{
    public Point Point { get; set; }

    public SurveyPoint(Point point)
    {
        Point = point;
    }

    public bool Equals(SurveyPoint other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(Point, other.Point);
        }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;

        return Equals((SurveyPoint)obj);
    }

    public override int GetHashCode()
    {
        return Point != null ? Point.GetHashCode() : 0;
    }

    public object Clone()
    {
        var surveyPoint = (SurveyPoint)MemberwiseClone();
        surveyPoint.Point = (Point)Point.Clone();
        return surveyPoint;
    }
}
