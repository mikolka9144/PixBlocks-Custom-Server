using System;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Pix_API.Base.Disk;
using Pix_API.Base.Utills;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class DiskReportObjects : SinglePoolStorageProvider<ObjectInArea>, IAreaObjectsProvider
    {
        private IdAssigner Index;

        public DiskReportObjects(DataSaver<ObjectInArea> saver,ILastIndexSaver indexSaver) : base(saver)
        {
            Index = new IdAssigner(indexSaver);
        }

        public int AddObject(ObjectInArea area)
        {
            area.Id = Index.NextEmptyId;
            AddSingleObject(area,area.Id);
            return area.Id;
        }

        public ObjectInArea GetObject(int Id) => GetSingleObject(Id);
        public new void RemoveObject(int Id) => base.RemoveObject(Id);
        public void UpdateObject(ObjectInArea area) => AddOrUpdateSingleObject(area, area.Id);
    }
}
