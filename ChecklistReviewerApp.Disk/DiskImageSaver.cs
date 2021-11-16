using System;
using System.IO;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.Base.Utills;
using NeoSmart.Utils;
namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class DiskImageSaver:IImageManager
    {
        private IdAssigner indexer;
        private readonly string picturesPath;

        public DiskImageSaver(string picturesPath,ILastIndexSaver saver)
        {
            indexer = new IdAssigner(saver);
            if (!Directory.Exists(picturesPath)) Directory.CreateDirectory(picturesPath);
            this.picturesPath = picturesPath;
        }

        public string GetBase64Image(int Id)
        {
            var bytes = File.ReadAllBytes(Path.Combine(picturesPath, Id.ToString()));
            return UrlBase64.Encode(bytes);
        }

        public void RemoveImage(int Id)
        {
            File.Delete(Path.Combine(picturesPath, Id.ToString()));
        }

        public int UploadBase64(string base64)
        {
            var bytes = UrlBase64.Decode(base64);
            var id = indexer.NextEmptyId;
            File.WriteAllBytes(Path.Combine(picturesPath, id.ToString()), bytes);
            return id;
        }

        public void EditImage(int Id, string base64)
        {
            var bytes = UrlBase64.Decode(base64);
            File.WriteAllBytes(Path.Combine(picturesPath, Id.ToString()), bytes);
        }
    }
}
