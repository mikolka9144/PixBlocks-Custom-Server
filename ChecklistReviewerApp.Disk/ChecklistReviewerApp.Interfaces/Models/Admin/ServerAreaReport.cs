using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class ServerObjectReport
    {
        public string ObjectName;
        public string description;
        public int ImageId;
    }
    [Serializable]
    public class ServerAreaReport
    {
        public int Id;
        public string Creator;
        public string CreationTime;
        public string AreaName;
        public List<ServerObjectReport> Objects;
    }
}
