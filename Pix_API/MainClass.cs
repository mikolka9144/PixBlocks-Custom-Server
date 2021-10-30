using System;
using System.Threading.Tasks;
using Ninject;
using Pix_API.PixBlocks;
using NLog;
using NLog.Config;
using Ninject.Modules;
using System.Collections.Generic;
using System.Threading;
using Pix_API.Base.Utills;

namespace Pix_API
{
	internal class MainClass
	{
		public static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            var serverConfiguration = ServerConfiguration.Read_configuration();
            kernel.Bind<ServerConfiguration>().ToConstant(serverConfiguration).InSingletonScope();
            kernel.Load<StartupModule>();
            kernel.Load(serverConfiguration.Providers_lib);
            kernel.Load(serverConfiguration.ApiResolver);

            List<ConnectionRecever> servers = Generate_servers(kernel, serverConfiguration);

            foreach (var server in servers)
            {
                Task.Run(new Action(server.Start_Lisening_Action));
            }
            Thread.Sleep(Timeout.Infinite);
        }

        private static List<ConnectionRecever> Generate_servers(StandardKernel kernel, ServerConfiguration serverConfiguration)
        {
            var servers = new List<ConnectionRecever>();
            foreach (var item in serverConfiguration.servers)
            {
                kernel.Load(item.lib);
                kernel.Bind<string>().ToConstant(item.host_ip).WhenInjectedExactlyInto<ConnectionRecever>();
                servers.Add(kernel.Get<ConnectionRecever>());
                kernel.Unbind<ICommandRepository>();
                kernel.Unbind<string>();
            }

            return servers;
        }
    }
    internal class StartupModule : NinjectModule
    {
        public override void Load()
        {
            var log_repo = new LogFactory();
            // Configure Console output
            SimpleConfigurator.ConfigureForConsoleLogging();
            log_repo.Configuration = LogManager.Configuration;
            //
            Bind<LogFactory>().ToConstant(log_repo);

        }
    }
}
