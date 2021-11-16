using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Disk;
using AdministrationApp.Models;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public static class Converters
    {
        public static ClientAreaToCheck ConvertAreaToClient(this ServerAreaToCheck serverArea,IAreaObjectsProvider objectsProvider,IImageManager imageManager)
        {
            var objs = new List<ClientObjectInArea>();
            serverArea.ObjectsInArea.ForEach(s => {
                var serverObj = objectsProvider.GetObject(s);
                var clientObj = new ClientObjectInArea
                {
                    Id = serverObj.Id,
                    image = imageManager.GetBase64Image(serverObj.ImageId),
                    name = serverObj.name
                };
                objs.Add(clientObj);
            });
            return new ClientAreaToCheck{
                Id = serverArea.Id,
                image = imageManager.GetBase64Image(serverArea.imageId),
                name = serverArea.name,
                ObjectsInArea = objs
            };
        }
        public static ServerAreaReport ConvertReport(this ClientAreaReport report, string Owner,IAreaMetadataProvider areaProvider,IAreaObjectsProvider objectsProvider,IImageManager imageManager)
        {
            var area = areaProvider.GetArea(report.AreaId);
            var res = new ServerAreaReport
            {
                AreaName = area.name,
                Objects = new List<ServerObjectReport>(),
                CreationTime = DateTime.Now.ToString(),
                Creator = Owner
            };
            foreach (var item in report.Objects)
            {
                var server_obj = objectsProvider.GetObject(item.ObjectId);
                var obj = new ServerObjectReport
                {
                    description = item.description,
                    ImageId = imageManager.UploadBase64(item.imageBase64),
                    ObjectName = server_obj.name
                };
                res.Objects.Add(obj);
            }
            return res;
        }
        public static ServerObjectInArea ConvertToServer(this ClientObjectInArea obj,int ImageId)
        {
            return new ServerObjectInArea
            {
                Id = obj.Id,
                name = obj.name,
                ImageId = ImageId
            };
        }
        public static ServerAreaToCheck ConvertToServer(this AdminAreaToCheck obj, IImageManager imageManager,int? ImageToOverride)
        {
            int ImageId;
            if (ImageToOverride != null)
            {
                imageManager.EditImage(ImageToOverride.Value, obj.image);
                ImageId = ImageToOverride.Value;
            }
            else ImageId = imageManager.UploadBase64(obj.image);
            return new ServerAreaToCheck
            {
                Id = obj.Id,
                imageId = ImageId,
                name = obj.name,
                ObjectsInArea = obj.ObjectsInArea,
                terrain = obj.terrain
            };
        }
    }
}
