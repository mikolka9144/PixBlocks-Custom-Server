using System;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IObjectReportSubmissions
    {
        void SubmitReport(AreaReport report, int UserId);
    }
}
