using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    public class AreaToCheck
    {
        public int Id;
        public string name;
        public string image;
        public List<ObjectInArea> ObjectsInArea;
    }
}
