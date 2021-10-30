using System.Collections.Generic;
using Newtonsoft.Json;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public class ClientAppCommands:ICommandRepository
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUsersProvider usersProvider;
        private readonly IAreaToCheckMetadataProvider areaProvider;
        private readonly IObjectReportSubmissions reportsProvider;

        public ClientAppCommands(ITokenProvider tokenProvider,IUsersProvider usersProvider,IAreaToCheckMetadataProvider areaProvider,IObjectReportSubmissions reportsProvider)
        {
            this.tokenProvider = tokenProvider;
            this.usersProvider = usersProvider;
            this.areaProvider = areaProvider;
            this.reportsProvider = reportsProvider;
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
        public List<AreaToCheck> Get_Areas(string token)
        {
            var areas = new List<AreaToCheck>();
            var user = usersProvider.GetUser(tokenProvider.GetUserForToken(token));
            foreach (var item in user.areasToCheckIds)
            {
                areas.Add(areaProvider.GetArea(item));
            }
            return areas;
        }
        public void Send_reports(string token,[FromBody] string jsonReport)
        {
            var userId = tokenProvider.GetUserForToken(token);
            var report = JsonConvert.DeserializeObject<AreaReport>(jsonReport);
            reportsProvider.SubmitReport(report, userId);
            var user = usersProvider.GetUser(userId);
            user.areasToCheckIds.Remove(report.AreaId);
            usersProvider.UpdateUser(user);
        }
    }
}
