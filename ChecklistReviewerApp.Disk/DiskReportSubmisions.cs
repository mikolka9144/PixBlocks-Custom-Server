using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Linq;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class DiskReportSubmisions : SinglePoolStorageProvider<ServerAreaReport>, IObjectReportSubmissions
    {
        private IdAssigner index;

        public DiskReportSubmisions(DataSaver<ServerAreaReport> saver,ILastIndexSaver lastIndex) : base(saver)
        {
            index = new IdAssigner(lastIndex);
        }

        public List<ServerAreaReport> GetAllReports() => storage.ToList();

        public ServerAreaReport GetReport(int id)
        {
            return GetSingleObject(id);
        }

        public void RemoveReport(int Id)
        {
            RemoveObject(Id);
        }

        public void SubmitReport(ServerAreaReport report)
        {
            report.Id = index.NextEmptyId;
            AddSingleObject(report, report.Id);
        }
    }
}
