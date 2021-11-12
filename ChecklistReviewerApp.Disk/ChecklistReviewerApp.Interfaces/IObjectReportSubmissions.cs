using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IObjectReportSubmissions
    {
        List<ServerAreaReport> GetAllReports();
        void SubmitReport(ServerAreaReport report);
        void RemoveReport(int Id);
    }
}
