using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace AdministrationApp.API
{
    public class CachedAPI:ServerAPI
    {
        private List<ServerAreaToCheck> areas;
        public new List<ServerAreaToCheck> GetAllAreasToCheck()
        {
            if (areas is null) areas = base.GetAllAreasToCheck();
            return areas;
        }
        public new ServerAreaToCheck AddArea(ServerAreaToCheck area)
        {
            var IDarea = base.AddArea(area);
            areas.Add(IDarea);
            return IDarea;
        }
        public new void EditArea(ServerAreaToCheck obj)
        {
            base.EditArea(obj);
            areas.RemoveAll(s => s.Id == obj.Id);
            areas.Add(obj);
        }
        public new void RemoveArea(int areaId)
        {
            base.RemoveArea(areaId);
            areas.RemoveAll(s => s.Id == areaId);
        }
    }
}
