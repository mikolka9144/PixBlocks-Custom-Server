using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace AdministrationApp.Models
{
    public class AdminAreaToCheck
    {
        public int Id;
        public string name;
        public string terrain;
        public string image;
        public List<int> ObjectsInArea;
    }
}
