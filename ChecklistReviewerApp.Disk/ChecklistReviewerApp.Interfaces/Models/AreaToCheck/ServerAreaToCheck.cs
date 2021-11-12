using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    public class ServerAreaToCheck
    {
        public int Id;
        public string name;
        public string terrain;
        public string image;
        public List<int> ObjectsInArea;
    }
}
