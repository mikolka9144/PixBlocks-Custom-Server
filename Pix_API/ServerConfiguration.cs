using System.IO;
using Newtonsoft.Json;

namespace Pix_API
{
	public class ServerConfiguration
	{
		public string server_host_ip;

		public string championship_server_host_ip;

		public string Providers_lib;

		public static ServerConfiguration read_configuration()
		{
			if (!File.Exists("Config.cfg"))
			{
				ServerConfiguration serverConfiguration = new ServerConfiguration
				{
					Providers_lib = "Pix_API.Providers.Disk.dll",
					server_host_ip = "http://*:8080/",
					championship_server_host_ip = "http://*:8081/"
				};
				File.WriteAllText("Config.cfg", JsonConvert.SerializeObject(serverConfiguration));
				return serverConfiguration;
			}
			return JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText("Config.cfg"));
		}
	}
}
