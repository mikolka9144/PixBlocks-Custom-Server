using System;
namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class ClientObjectInArea
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string image;
    }
    [Serializable]
    public class ServerObjectInArea
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int ImageId;
    }//TODO
}
