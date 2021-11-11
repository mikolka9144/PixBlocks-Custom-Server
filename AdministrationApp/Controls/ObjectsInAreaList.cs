using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;
using System.Linq;
using AdministrationApp.Controls;
namespace AdministrationApp
{
    public class ObjectsInAreaList:ObjectsList<ObjectInArea>
    {
        public void AddObject(ObjectInArea area)
        {
            AddObject(area, area.name);
        }
        public void RemoveObject(ObjectInArea area)
        {
            RemoveObject(s =>
            {
                return s.Id == area.Id;
            });
        }
        public void EditObject(ObjectInArea area)
        {
            RemoveObject(area);
            Items.Insert(0, area, area.name);
            SelectRow(0);
        }

        public List<int> GetAllObjectsIDs()
        {
            return Items.Cast<ObjectInArea>().Select(s => s.Id).ToList();
        }
    }
}
