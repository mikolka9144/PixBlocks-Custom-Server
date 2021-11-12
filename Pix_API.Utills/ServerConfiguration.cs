using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pix_API.Base.Utills
{
	public class ServerConfiguration
	{
        public List<ServerInstanceConfiguration> servers;

		public string Providers_lib;
        public string ApiResolver;

        public static ServerConfiguration Read_configuration()
		{
			if (!File.Exists("Config.cfg"))
			{
                ServerConfiguration serverConfiguration = new ServerConfiguration
                {
                    servers = new List<ServerInstanceConfiguration>
                    {
                        new ServerInstanceConfiguration
                        {

                            host_ip = "http://*:8080/"
                        },
                    },
                    ApiResolver = "BaseCommandResolver.dll"
				};
				File.WriteAllText("Config.cfg", JsonConvert.SerializeObject(serverConfiguration));
                Console.WriteLine("New Configuration has been created. Make sure that settings are set correctly and restart server.");
                Environment.Exit(0);
			}
			return JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText("Config.cfg"));
		}
	}
    public class ServerInstanceConfiguration
    {
        public string lib;
        public string host_ip;
    }
}
