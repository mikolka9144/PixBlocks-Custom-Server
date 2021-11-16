using System;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public interface IAreaObjectsProvider
    {
        ServerObjectInArea GetObject(int Id);
        int AddObject(ServerObjectInArea area);
        void RemoveObject(int Id);
        void UpdateObject(ServerObjectInArea area);
    }
}
