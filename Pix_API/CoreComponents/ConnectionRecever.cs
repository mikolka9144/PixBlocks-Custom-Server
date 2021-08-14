using System;
using System.Net;
using System.Text;

namespace Pix_API.CoreComponents
{
    public delegate void ConnectionArgs(string method_name,string[] parameters,HttpListenerResponse response);
    public class ConnectionRecever
    {
        public event ConnectionArgs OnCommandReceved;
        public void Start_Lisening(string adress)
        {
            var lisner = new HttpListener();
            lisner.Prefixes.Add(adress);
            lisner.Start();
            while (true)
            {
                var request = lisner.GetContext();
                var server_fasade_method_name = request.Request.QueryString["me"];
                var parameters = new string[]{
                    request.Request.QueryString["p1"],
                    request.Request.QueryString["p2"],
                    request.Request.QueryString["p3"],
                    request.Request.QueryString["p4"],
                    request.Request.QueryString["p5"],
                    request.Request.QueryString["p6"],
                    request.Request.QueryString["p7"]
                    };
                request.Response.ContentType = "application/json";
                request.Response.ContentEncoding = Encoding.UTF8;
                request.Response.SendChunked = false;
                OnCommandReceved(server_fasade_method_name, parameters,request.Response);
                request.Response.Close();
            }
        }
    }
}
