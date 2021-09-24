using System;
using System.Threading.Tasks;
using Ninject;
using Pix_API.CoreComponents;
using Pix_API.CoreComponents.ServerCommands;
using Pix_API.Interfaces;
using Pix_API.Utills;
using NLog;
using System.IO;
using NLog.Config;
using Ninject.Parameters;
using Ninject.Modules;

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

            var connectionRecever = create_Server(kernel, typeof(Main_Logic), serverConfiguration.server_host_ip);
			var championship_server = create_Server(kernel, typeof(ChampionshipAPICommands), serverConfiguration.championship_server_host_ip);

            Task.Run(() => championship_server.Start_Lisening_Action());
			connectionRecever.Start_Lisening_Action();
		}
        private static ConnectionRecever create_Server(IKernel kernel,Type repo,string adress)
        {
            kernel.Bind<ICommandRepository>().To(repo);
            kernel.Bind<string>().ToConstant(adress).WhenInjectedExactlyInto<ConnectionRecever>();
            var connectionRecever = kernel.Get<ConnectionRecever>();
            kernel.Unbind<ICommandRepository>();
            kernel.Unbind<string>();
            return connectionRecever;
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
            Bind<IAbstractUser>().To<ChampionshipsUser>();
        }
    }
}
