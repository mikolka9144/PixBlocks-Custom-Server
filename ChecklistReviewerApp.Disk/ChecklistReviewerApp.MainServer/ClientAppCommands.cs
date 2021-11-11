using System.Collections.Generic;
using Newtonsoft.Json;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Pix_API.ChecklistReviewerApp.Disk;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public partial class ClientAppCommands:ICommandRepository
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUsersProvider usersProvider;
        private readonly IAreaToCheckMetadataProvider areaProvider;
        private readonly IObjectReportSubmissions reportsProvider;
        private readonly IAreaObjectsProvider objectsProvider;

        public ClientAppCommands(ITokenProvider tokenProvider,IUsersProvider usersProvider,IAreaToCheckMetadataProvider areaProvider,IObjectReportSubmissions reportsProvider,IAreaObjectsProvider objectsProvider)
        {
            this.tokenProvider = tokenProvider;
            this.usersProvider = usersProvider;
            this.areaProvider = areaProvider;
            this.reportsProvider = reportsProvider;
            this.objectsProvider = objectsProvider;
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
                var objs = new List<ObjectInArea>();
                ServerArea.ObjectsInArea.ForEach(s => objs.Add(objectsProvider.GetObject(s)));
                areas.Add(new ClientAreaToCheck(ServerArea, objs));
            }
            return areas;
        }
        public void Send_reports(string token,[FromBody] AreaReport report)
        {
            var userId = tokenProvider.GetUserForToken(token);
            reportsProvider.SubmitReport(report, userId);
            var user = usersProvider.GetUser(userId);
            user.areasToCheckIds.Remove(report.AreaId);
            usersProvider.UpdateUser(user);
        }
    }
}
