using Ninject.Modules;
using Pix_API.Base.CommandResolver;
using Pix_API.Base.Utills;

namespace BaseCommandResolver
{
    public class NinjectManifest:NinjectModule
    {
        public NinjectManifest()
        {
        }

        public override void Load()
        {
            Bind<IAPIServerResolver>().To<APIServerResolver>();
        }
    }
}
