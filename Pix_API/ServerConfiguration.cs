using System;
using System.IO;
using Newtonsoft.Json;

namespace Pix_API
{
	public class ServerConfiguration
	{
		public string server_host_ip;

		public string championship_server_host_ip;

		public string Providers_lib;
        public string ChampionshipUser_Password;

		public static ServerConfiguration Read_configuration()
		{
			if (!File.Exists("Config.cfg"))
			{
                ServerConfiguration serverConfiguration = new ServerConfiguration
                {
                    Providers_lib = "Pix_API.Providers.Disk.dll",
                    server_host_ip = "http://*:8080/",
                    championship_server_host_ip = "http://*:8081/",
                    ChampionshipUser_Password = new Random().Next().ToString()
				};
				File.WriteAllText("Config.cfg", JsonConvert.SerializeObject(serverConfiguration));
                Console.WriteLine("New Configuration has been created. Make sure that settings are set correctly and restart server.");
                Environment.Exit(0);
			}
			return JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText("Config.cfg"));
		}
	}
}
