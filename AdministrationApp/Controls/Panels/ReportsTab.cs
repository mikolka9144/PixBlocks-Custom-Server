using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;

namespace AdministrationApp.Controls.Panels
{
    public class ReportsTab:VBox
    {
        private readonly IAPIClient client;
        private ReportsTable ReportsTable;

        public ReportsTab(IAPIClient client)
        {
            ReportsTable = new ReportsTable(client.GetAllReports());
            PackStart(ReportsTable,true);
            PackEnd(RemoveBtn());
            this.client = client;
        }
        private Button RemoveBtn()
        {
            var btn = new Button("Remove report");
            btn.Clicked += delegate {
                var report = ReportsTable.GetSelectedReport();
                client.RemoveReport(report.Id);
                ReportsTable.RemoveReport(report);
            };
            return btn;
        }
    }
}
