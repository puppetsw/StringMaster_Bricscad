using Point = StringMaster.UI.Models.Point;

namespace StringMaster_Bricscad.extensions;

public static class CogoPointExtensions
{
    public static Point ToPoint(this Bricscad.Civil.Point cogoPoint) => new(cogoPoint.Easting, cogoPoint.Northing, cogoPoint.Elevation);
}