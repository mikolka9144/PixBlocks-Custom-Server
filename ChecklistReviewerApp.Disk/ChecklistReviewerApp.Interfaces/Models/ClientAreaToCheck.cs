using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    public class ClientAreaToCheck
    {
        public int Id;
        public string name;
        public string image;
        public List<ObjectInArea> ObjectsInArea;

        public ClientAreaToCheck()
        {

        }
        public ClientAreaToCheck(ServerAreaToCheck serverArea, List<ObjectInArea> objsToCheck)
        {
            Id = serverArea.Id;
            name = serverArea.name;
            image = serverArea.image;
            ObjectsInArea = objsToCheck;
        }
    }
}
