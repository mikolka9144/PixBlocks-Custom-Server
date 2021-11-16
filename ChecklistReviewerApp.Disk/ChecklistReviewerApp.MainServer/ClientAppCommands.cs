using System.Collections.Generic;
using Newtonsoft.Json;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Pix_API.ChecklistReviewerApp.Disk;
using System;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public partial class ClientAppCommands:ICommandRepository
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUsersProvider usersProvider;
        private readonly IAreaMetadataProvider areaProvider;
        private readonly IObjectReportSubmissions reportsProvider;
        private readonly IAreaObjectsProvider objectsProvider;
        private readonly IImageManager imageManager;

        public ClientAppCommands(ITokenProvider tokenProvider,IUsersProvider usersProvider,IAreaMetadataProvider areaProvider,IObjectReportSubmissions reportsProvider,IAreaObjectsProvider objectsProvider,IImageManager imageManager)
        {
            this.tokenProvider = tokenProvider;
            this.usersProvider = usersProvider;
            this.areaProvider = areaProvider;
            this.reportsProvider = reportsProvider;
            this.objectsProvider = objectsProvider;
            this.imageManager = imageManager;
        }
        public string Get_token(string login,string passwordHash)
        {
            var user = usersProvider.GetUser(login);
            if(user != null)
            {
                if (user.passwordHash == passwordHash) return tokenProvider.GetTokenForUser(user.Id);
            }
            return null;
        }
        public List<ClientAreaToCheck> Get_Areas(string token)
        {
            var areas = new List<ClientAreaToCheck>();
            var user = usersProvider.GetUser(tokenProvider.GetUserForToken(token));
            foreach (var item in user.areasToCheckIds)
            {
                var ServerArea = areaProvider.GetArea(item);
                areas.Add(ServerArea.ConvertAreaToClient(objectsProvider, imageManager));
            }
            return areas;
        }
        public void Send_reports(string token,[FromBody] ClientAreaReport report)
        {
            var userId = tokenProvider.GetUserForToken(token);
            var user = usersProvider.GetUser(userId);
            reportsProvider.SubmitReport(report.ConvertReport(user.login,areaProvider,objectsProvider,imageManager));

            user.areasToCheckIds.Remove(report.AreaId);
            usersProvider.UpdateUser(user);
        }

    }
}
