using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Linq;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class FileAreaToCheckMatadata : SinglePoolStorageProvider<ServerAreaToCheck>, IAreaMetadataProvider
    {
        IdAssigner idAssigner;
        public FileAreaToCheckMatadata(DataSaver<ServerAreaToCheck> saver,ILastIndexSaver index_saver) : base(saver)
        {
            idAssigner = new IdAssigner(index_saver);
        }

        public void AddArea(ServerAreaToCheck area)
        {
            area.Id = idAssigner.NextEmptyId;
            AddSingleObject(area, area.Id);
        }

        public void EditArea(ServerAreaToCheck area)
        {
            AddOrUpdateSingleObject(area, area.Id);
        }

        public List<ServerAreaToCheck> GetAllAreas()
        {
            return storage.ToList();
        }

        public ServerAreaToCheck GetArea(int Id)
        {
            return GetSingleObject(Id);
        }

        public void RemoveArea(int Id)
        {
            RemoveObject(Id);
        }
    }
}
