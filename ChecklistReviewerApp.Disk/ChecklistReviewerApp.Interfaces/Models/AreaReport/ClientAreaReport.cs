using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Disk;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class ClientObjectReport
    {
        public int ObjectId;
        public string description;
        public string imageBase64;
    }
    [Serializable]
    public class ClientAreaReport
    {
        public int Id;
        public int AreaId;
        public List<ClientObjectReport> Objects;
    }
}
