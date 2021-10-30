using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class DiskReportSubmisions : MultiplePoolStorageProvider<AreaReport>, IObjectReportSubmissions
    {
        private IdAssigner index;

        public DiskReportSubmisions(DataSaver<List<AreaReport>> saver,ILastIndexSaver lastIndex) : base(saver)
        {
            index = new IdAssigner(lastIndex);
        }

        public void SubmitReport(AreaReport report, int UserId)
        {
            report.Id = index.NextEmptyId;
            AddObject(report, UserId);
        }
    }
}
