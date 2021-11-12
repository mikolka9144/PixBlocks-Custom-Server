using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;

namespace AdministrationApp.Controls
{
    public class ReportsTable:ListView
    {

        private DataField<string> AreaName = new DataField<string>();
        private DataField<string> CreatorName = new DataField<string>();
        private DataField<DateTime> CreationDate = new DataField<DateTime>();
        private List<ServerAreaReport> Reports = new List<ServerAreaReport>();
        private ListStore DataStore;

        public ReportsTable(List<ServerAreaReport> reports)
        {
            DataStore = new ListStore(AreaName, CreatorName, CreationDate);
            DataSource = DataStore;
            Columns.Add("Creator", CreatorName);
            Columns.Add("Area", AreaName);
            Columns.Add("Creation date", CreationDate);
            reports.ForEach(AddReport);
        }
        private void AddReport(ServerAreaReport report)
        {
            var index = DataStore.AddRow();
            Reports.Add(report);
            DataStore.SetValues(index, CreatorName, report.Creator, AreaName, report.AreaName, CreationDate, report.CreationTime);
        }
        public void RemoveReport(ServerAreaReport report)
        {
            var index = Reports.IndexOf(report);
            Reports.RemoveAt(index);
            DataStore.RemoveRow(index);
        }
        public ServerAreaReport GetSelectedReport()
        {
            return Reports[SelectedRow];
        }
    }
}
