using Bricscad.Civil;
using Bricscad.CivilExtensions;
using StringMaster.Common.Extensions;
using StringMaster.UI.Services.Interfaces;
using Teigha.DatabaseServices;

namespace StringMaster.Common.Services.Implementation
{
    public class CivilPointGroupService : ICivilPointGroupService
    {
        public IEnumerable<string> GetPointGroups()
        {
            var db = AcadApplicationService.ActiveDatabase;
            var docMan = AcadApplicationService.DocumentManager;
            var doc = docMan.CurrentDocument;

            using Transaction tr = doc.TransactionManager.StartLockedTransaction();
            ObjectId managerId = doc.Database.GetCivilPointGroupManager(true);
            var mgr = tr.GetObject(managerId, OpenMode.ForWrite) as PointGroupManager;
            var allPoints = tr.GetObject(mgr.Find("_All points"), OpenMode.ForRead) as PointGroup;

            foreach (string name in mgr.Names)
            {
                yield return name;
            }

            tr.Commit();
        }
    }
}
