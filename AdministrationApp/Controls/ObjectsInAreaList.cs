using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;
using System.Linq;
using AdministrationApp.Controls;
namespace AdministrationApp
{
    public class ObjectsInAreaList:ObjectsList<ServerObjectInArea>
    {
        public void AddObject(ServerObjectInArea area)
        {
            AddObject(area, area.name);
        }
        public void RemoveObject(ServerObjectInArea area)
        {
            RemoveObject(s =>
            {
                return s.Id == area.Id;
            });
        }
        public void EditObject(ServerObjectInArea area)
        {
            RemoveObject(area);
            Items.Insert(0, area, area.name);
            SelectRow(0);
        }

        public List<int> GetAllObjectsIDs()
        {
            return Items.Cast<ServerObjectInArea>().Select(s => s.Id).ToList();
        }
    }
}
