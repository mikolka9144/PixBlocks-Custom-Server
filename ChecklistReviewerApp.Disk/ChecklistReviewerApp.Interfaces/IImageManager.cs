using System;
namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IImageManager
    {
        string GetBase64Image(int Id);
        int UploadBase64(string base64);
        void RemoveImage(int Id);
        void EditImage(int Id,string base64);
    }
}
