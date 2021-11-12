using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class ServerObjectReport
    {
        public string ObjectName;
        public string description;
        public string imageBase64;
    }
    [Serializable]
    public class ServerAreaReport
    {
        public int Id;
        public string Creator;
        public DateTime CreationTime;
        public string AreaName;
        public List<ServerObjectReport> Objects;
    }
}
