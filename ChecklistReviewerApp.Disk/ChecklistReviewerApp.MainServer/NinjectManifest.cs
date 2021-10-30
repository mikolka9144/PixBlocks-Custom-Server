using Ninject.Modules;
using Pix_API.Base.Utills;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public class NinjectManifest:NinjectModule
    {

        public override void Load()
        {
            Bind<ICommandRepository>().To<ClientAppCommands>();
        }
    }
}
