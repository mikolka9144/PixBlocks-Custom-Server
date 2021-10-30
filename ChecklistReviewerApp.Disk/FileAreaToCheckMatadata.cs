using Pix_API.Base.Disk;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class FileAreaToCheckMatadata : SinglePoolStorageProvider<AreaToCheck>, IAreaToCheckMetadataProvider
    {
        IdAssigner idAssigner;
        public FileAreaToCheckMatadata(DataSaver<AreaToCheck> saver,ILastIndexSaver index_saver) : base(saver)
        {
            idAssigner = new IdAssigner(index_saver);
        }

        public void AddArea(AreaToCheck area)
        {
            area.Id = idAssigner.NextEmptyId;
            AddSingleObject(area, area.Id);
        }

        public AreaToCheck GetArea(int Id)
        {
            return GetSingleObject(Id);
        }
    }
}
