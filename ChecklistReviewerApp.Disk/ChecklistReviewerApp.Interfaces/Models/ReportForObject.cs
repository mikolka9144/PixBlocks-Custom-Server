using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class ReportForObject
    {
        public int ObjectId;
        public string description;
        public string imageBase64;
    }
    [Serializable]
    public class AreaReport
    {
        public int Id;
        public int AreaId;
        public List<ReportForObject> Objects;
    }
}
