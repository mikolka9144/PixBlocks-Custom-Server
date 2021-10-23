using System;
using Ninject.Modules;
using Pix_API.CoreComponents;
using Pix_API;
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
