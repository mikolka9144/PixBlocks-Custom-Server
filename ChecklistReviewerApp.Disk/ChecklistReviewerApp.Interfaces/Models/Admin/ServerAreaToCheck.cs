using System;
using System.Collections.Generic;
using AdministrationApp.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    public class ServerAreaToCheck
    {
        public int Id;
        public string name;
        public string terrain;
        public int imageId;
        public List<int> ObjectsInArea;
    }
}
