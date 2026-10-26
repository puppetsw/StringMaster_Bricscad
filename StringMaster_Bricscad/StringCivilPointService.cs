using Bricscad.ApplicationServices;
using Bricscad.Civil;
using Bricscad.CivilExtensions;
using StringMaster_Bricscad.extensions;
using StringMaster.Common.Extensions;
using StringMaster.Common.Helpers;
using StringMaster.UI.Models;
using StringMaster.UI.Services.Interfaces;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Point = Bricscad.Civil.Point;

namespace StringMaster_Bricscad;

public class StringCivilPointService : IStringCivilPointsService
{
	public void StringCivilPoints(IList<DescriptionKey> descriptionKeys, string filterByPointGroup = "")
	{
		var doc = Application.DocumentManager.MdiActiveDocument;

		if (doc == null)
        {
            return;
        }

        using Transaction tr = doc.TransactionManager.StartLockedTransaction();
        var desMapping = new Dictionary<string, DescriptionKeyMatch>();


        /*_AcDb.ObjectId managerId = db.GetCivilPointGroupManager(true);
        Trace.Assert(!managerId.IsNull, "Manager id should not be null");
        _AcCiv.PointGroupManager mgr = tr.GetObject(managerId, _AcDb.OpenMode.ForWrite) as _AcCiv.PointGroupManager;
        Trace.Assert(1 == mgr.ElementCount, "By default there should be only one point group '_All points'.");
        _AcCiv.PointGroup groupAllPoints = tr.GetObject(mgr.Find("_All points"), _AcDb.OpenMode.ForRead) as _AcCiv.PointGroup;*/

        ObjectId managerId = doc.Database.GetCivilPointGroupManager(true);
        var mgr = tr.GetObject(managerId, OpenMode.ForWrite) as PointGroupManager;
        var allPoints = tr.GetObject(mgr.Find("_All points"), OpenMode.ForRead) as PointGroup;

        foreach (ObjectId pointId in allPoints.Points)
		{
			var cogoPoint = pointId.GetObject(OpenMode.ForRead) as Point;
			if (cogoPoint == null)
            {
                continue;
            }

            /*// Check if we need to filter by PointGroup
            if (!string.IsNullOrEmpty(filterByPointGroup))
            {
                var pointGroup = cogoPoint.PrimaryPointGroupId.GetObject(OpenMode.ForRead) as PointGroup;
                if (pointGroup != null && !pointGroup.Name.Equals(filterByPointGroup, StringComparison.OrdinalIgnoreCase))
                {
                    // Skip this point. It doesn't belong to the specified PointGroup
                    continue;
                }
            }*/

            var rawDescription = cogoPoint.RawDescription;

			foreach (DescriptionKey descriptionKey in descriptionKeys)
			{
				if (!DescriptionKeyMatch.IsMatch(rawDescription, descriptionKey))
                {
                    continue;
                }

                string description = DescriptionKeyMatch.Description(rawDescription, descriptionKey);
				string lineNumber = DescriptionKeyMatch.LineNumber(rawDescription, descriptionKey);

				var mapKey = $"{description}|{descriptionKey.Id}";

				if (!desMapping.TryGetValue(mapKey, out var deskeyMatch))
				{
					deskeyMatch = new DescriptionKeyMatch(descriptionKey);
					desMapping.Add(mapKey, deskeyMatch);
				}

				deskeyMatch.AddPoint(cogoPoint.ToPoint(), lineNumber);
			}
		}

		var bt = (BlockTable) tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
		var btr = (BlockTableRecord) tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

		foreach (var desKey in desMapping)
		{
			DescriptionKeyMatch deskeyMatch = desKey.Value;

			foreach (var surveyPoints in deskeyMatch.SurveyPoints)
			{
				var list = surveyPoints.Value;
				if (list == null || list.Count == 0)
                {
                    continue;
                }

                string layerName = deskeyMatch.DescriptionKey.Layer;

				if (deskeyMatch.DescriptionKey.AcadLayer.IsSelected)
				{
					if (!LayerHelpers.HasLayer(layerName, tr, doc.Database))
					{
						LayerHelpers.CreateLayer(deskeyMatch.DescriptionKey.AcadLayer, tr, doc.Database);
					}
				}
				else
				{
					layerName = "0"; // use default layer if not selected.
				}

				var pointCollection = new Point3dCollection();

				foreach (SurveyPoint surveyPoint in list)
                {
                    pointCollection.Add(surveyPoint.Point.ToPoint3d());
                }

				if (pointCollection.Count <= 1)
                {
                    continue;
                }

				if (deskeyMatch.DescriptionKey.Draw2D)
                {
                    PolylineHelpers.DrawPolyline2d(tr, btr, pointCollection, layerName, deskeyMatch.DescriptionKey.AcadColor.ToColor());
                }

                if (deskeyMatch.DescriptionKey.Draw3D)
                {
                    PolylineHelpers.DrawPolyline3d(tr, btr, pointCollection, layerName, deskeyMatch.DescriptionKey.AcadColor.ToColor());
                }
            }
		}
		tr.Commit();
	}
}