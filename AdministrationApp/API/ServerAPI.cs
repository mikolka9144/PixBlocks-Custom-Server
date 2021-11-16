using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using RestSharp;
using AdministrationApp.Models;

namespace AdministrationApp.API
{
    public class ServerAPI : IAPIClient
    {
        private const string ADRESS = "10.10.50.200:8080";
        private string token = "";
        private RestClient client;
        public ServerAreaToCheck AddArea(AdminAreaToCheck area)
        {
            return SendRequest<ServerAreaToCheck>("AddArea", new Parameter[0], area);
        }

        public ServerObjectInArea AddObject(ClientObjectInArea obj)
        {
            return SendRequest<ServerObjectInArea>("AddObject", new Parameter[0], obj);
        }

        public ServerAreaToCheck EditArea(AdminAreaToCheck obj)
        {
            return SendRequest<ServerAreaToCheck>("EditArea", new Parameter[0], obj);
        }

        public List<ServerAreaToCheck> GetAllAreasToCheck()
        {
            return SendRequest<List<ServerAreaToCheck>>("GetAllAreasToCheck", new Parameter[0]);
        }

        public List<User> GetAllUsers()
        {
            return SendRequest<List<User>>("GetAllUsers", new Parameter[0]);
        }

        public ServerObjectInArea GetObject(int Id)
        {
            return SendRequest<ServerObjectInArea>("GetObject", new[]
            {
                new Parameter("Id", Id,ParameterType.QueryString),
            });
        }

        public bool IsServerOnline()
        {
            var ping_request = new TcpClient();
            try
            {
                var seg = ADRESS.Split(':');
                ping_request.Connect(seg[0], Convert.ToInt32(seg[1]));
                ping_request.Close();
                return true;
            }
            catch { return false; }
        }

        public bool LoginToServer(string text, string password)
        {
            client = new RestClient($"http://{ADRESS}/");
            var res = SendRequest<string>("Get_token", new[]
            {
                new Parameter("login", text,ParameterType.QueryString),
                new Parameter("passwordHash",password,ParameterType.QueryString)
            }
            , null, false);
            if (res != null)
                if (IsTokenAdmin(res))
                {
                    token = res;
                    return true;
                }
            return false;
        }

        private bool IsTokenAdmin(string token2)
        {
            return SendRequest<bool>("IsAdmin", new[]
            {
                new Parameter("token", token2,ParameterType.QueryString)
            }
            , null, false);
        }

        public void RemoveObject(int Id)
        {
            SendRequest<string>("RemoveObject", new[]
            {
                new Parameter("Id", Id,ParameterType.QueryString),
            });
        }

        public void RemoveArea(int areaId)
        {
            SendRequest<string>("RemoveArea", new[]
            {
                new Parameter("areaId", areaId,ParameterType.QueryString),
            });
        }

        public ServerObjectInArea UpdateObject(ClientObjectInArea area)//TODO
        {
            return SendRequest<ServerObjectInArea>("UpdateObject", new Parameter[0], area);
        }
        private T SendRequest<T>(RestRequest request)
        {
            var response = client.Post(request);
            if (!response.IsSuccessful) return default(T);
            return SimpleJson.DeserializeObject<T>(response.Content);

        }
        private T SendRequest<T>(string command, IEnumerable<Parameter> extra_parameters, object body = null, bool IncludeToken = true)
        {
            var req = new RestRequest();
            req.Parameters.Add(new Parameter("command", command, ParameterType.QueryString));
            if (IncludeToken) req.Parameters.Add(new Parameter("token", token, ParameterType.QueryString));
            req.Parameters.AddRange(extra_parameters);
            req.AddJsonBody(body);
            return SendRequest<T>(req);
        }

        public void EditUser(User user)
        {
            SendRequest<object>("EditUser", new Parameter[0], user);
        }

        public void RemoveUser(int userId)
        {
            SendRequest<string>("RemoveUser", new[]
            {
                new Parameter("userId", userId,ParameterType.QueryString),
            });
        }

        public User AddUser(User user)
        {
            var res = SendRequest<int>("AddUser", new Parameter[0], user);
            user.Id = res;
            return user;
        }

        public List<ServerAreaReport> GetAllReports()
        {
            var res = SendRequest<List<ServerAreaReport>>("GetAllReports", new Parameter[0]
            );
            return res;
        }

        public void RemoveReport(int id)
        {
            SendRequest<string>("RemoveReport", new[]
            {
                new Parameter("userId", id,ParameterType.QueryString),
            });
        }
        public string GetImage( int ImageId)
        {
            return SendRequest<string>("GetImage", new[]
            {
                new Parameter("ImageId", ImageId,ParameterType.QueryString),
            });
        }
    }
}
