using System;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public interface IAreaObjectsProvider
    {
        ObjectInArea GetObject(int Id);
        int AddObject(ObjectInArea area);
        void RemoveObject(int Id);
        void UpdateObject(ObjectInArea area);
    }
}
