using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using RestSharp;

namespace AdministrationApp.API
{
    public class ServerAPI : IAPIClient
    {
        private const string ADRESS = "localhost:8080";
        private string token = "";
        private RestClient client;
        public ServerAreaToCheck AddArea(ServerAreaToCheck area)
        {
            var res = SendRequest<int>("AddArea", new Parameter[0],area);
            area.Id = res;
            return area;
        }

        public ObjectInArea AddObject(ObjectInArea obj)
        {
            var res = SendRequest<int>("AddObject", new Parameter[0], obj);
            obj.Id = res;
            return obj;
        }

        public void EditArea(ServerAreaToCheck obj)
        {
            SendRequest<object>("EditReport", new Parameter[0], obj);
        }

        public List<ServerAreaToCheck> GetAllAreasToCheck()
        {
            var res = SendRequest<List<ServerAreaToCheck>>("GetAllAreasToCheck", new Parameter[0]
            );
            return res;
        }

        public List<User> GetAllUsers()
        {
            var res = SendRequest<List<User>>("GetAllUsers", new Parameter[0]
            );
            return res;
        }

        public ObjectInArea GetObject(int Id)
        {
            return SendRequest<ObjectInArea>("GetObject", new[]
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
            ,null,false);
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
            var res = SendRequest<bool>("IsAdmin", new[]
            {
                new Parameter("token", token2,ParameterType.QueryString)
            }
            ,null,false);
            return res;
        }

        public void RemoveObject(int Id)
        {
            var res = SendRequest<string>("RemoveObject", new[]
            {
                new Parameter("Id", Id,ParameterType.QueryString),
            });
        }

        public void RemoveArea(int areaId)
        {
            var res = SendRequest<string>("RemoveReport", new[]
            {
                new Parameter("areaId", areaId,ParameterType.QueryString),
            });
        }

        public void UpdateObject(ObjectInArea area)
        {
            SendRequest<object>("UpdateObject", new Parameter[0],area);
        }
        private T SendRequest<T>(RestRequest request)
        {
            var response = client.Post(request);
            if (!response.IsSuccessful) return default(T);
            return SimpleJson.DeserializeObject<T>(response.Content);
          
        }
        private T SendRequest<T>(string command,IEnumerable<Parameter> extra_parameters,object body = null,bool IncludeToken = true)
        {
            var req = new RestRequest();
            req.Parameters.Add(new Parameter("command", command,ParameterType.QueryString));
            if(IncludeToken) req.Parameters.Add(new Parameter("token", token, ParameterType.QueryString));
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
            var res = SendRequest<string>("RemoveUser", new[]
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
            var res = SendRequest<string>("RemoveReport", new[]
            {
                new Parameter("userId", id,ParameterType.QueryString),
            });
        }
    }


}
