using Xwt.Drawing;
using System.IO;
using NeoSmart.Utils;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using AdministrationApp.Models;

namespace AdministrationApp
{
    public static class Utills
    {
        private const double targetWight = 400;
        public static Image TransformImage(Image image)
        {
            var scale = image.Width / targetWight;
            var targetHeight = image.Height / scale;
            return image.WithSize(targetWight, targetHeight);
        }
        public static Image GetImageFromBase64(string base64)
        {
            var bytes = UrlBase64.Decode(base64);
            return Image.FromStream(new MemoryStream(bytes));
        }
        public static string GetBase64FromImage(Image image)
        {
            var stream = new MemoryStream();
            image.Save(stream,ImageFileType.Jpeg);
            var base64 = UrlBase64.Encode(stream.ToArray());
            return base64;
        }
        public static AdminAreaToCheck ToAdmin(this ServerAreaToCheck area,IAPIClient client)
        {
            return new AdminAreaToCheck
            {
                Id = area.Id,
                image = client.GetImage(area.imageId),
                name = area.name,
                ObjectsInArea = area.ObjectsInArea,
                terrain = area.terrain
            };
        }
    }
}
