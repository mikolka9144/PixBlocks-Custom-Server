using NUnit.Framework;
using System;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.MainServer;
using Moq;
using Pix_API.ChecklistReviewerApp.Disk;

namespace ChecklistTests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestCompilingAreaForClient()
        {
            var tokens = new Mock<ITokenProvider>();
            tokens.Setup(s => s.GetUserForToken("")).Returns(0);
            var users = new Mock<IUsersProvider>();
            users.Setup(s => s.GetUser(0)).Returns(new User { Id = 0, areasToCheckIds = new List<int> { 0 } });
            var areas = new Mock<IAreaMetadataProvider>();
            areas.Setup(s => s.GetArea(0)).Returns(new ServerAreaToCheck { imageId = 0, ObjectsInArea = new List<int> { 0 } });
            var objects = new Mock<IAreaObjectsProvider>();
            objects.Setup(s => s.GetObject(0)).Returns(new ServerObjectInArea { ImageId = 1 });
            var images = new Mock<IImageManager>();
            images.Setup(s => s.GetBase64Image(0)).Returns("Image0");
            images.Setup(s => s.GetBase64Image(1)).Returns("Image1");

            var server = new ClientAppCommands(tokens.Object, users.Object, areas.Object, null, objects.Object, images.Object);
            var resultAreas = server.Get_Areas("");

            Assert.AreEqual("Image0", resultAreas[0].image);
            Assert.AreEqual("Image1", resultAreas[0].ObjectsInArea[0].image);
        }
        [Test]
        public void TestSendingReportImages()
        {
            var tokens = new Mock<ITokenProvider>();
            tokens.Setup(s => s.GetUserForToken("")).Returns(0);
            var users = new Mock<IUsersProvider>();
            users.Setup(s => s.GetUser(0)).Returns(new User { Id = 0, areasToCheckIds = new List<int> { 0 } });
            var areas = new Mock<IAreaMetadataProvider>();
            areas.Setup(s => s.GetArea(0)).Returns(new ServerAreaToCheck { imageId = 0, ObjectsInArea = new List<int> { 0 } });
            var objects = new Mock<IAreaObjectsProvider>();
            objects.Setup(s => s.GetObject(0)).Returns(new ServerObjectInArea { ImageId = 1 });
            var images = new Mock<IImageManager>();
            images.Setup(s => s.UploadBase64("ads")).Returns(0);
            var reports = new Mock<IObjectReportSubmissions>();
            var server = new ClientAppCommands(tokens.Object, users.Object, areas.Object, reports.Object, objects.Object, images.Object);
            server.Send_reports("",
            new ClientAreaReport
            {
                Objects = new List<ClientObjectReport> 
                {
                    new ClientObjectReport { imageBase64 = "ads" }
                }

            });

            Assert.AreEqual("ads", images.Invocations[0].Arguments[0] as string);
        }
    }
}
