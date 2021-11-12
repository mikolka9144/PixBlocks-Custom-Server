using Ninject.Modules;
using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class NinjectManifest:NinjectModule
    {
        public override void Load()
        {
            var user_saver = new DiskDataSaver<User>("./users");
            var area_saver = new DiskDataSaver<ServerAreaToCheck>("./areas");
            var report_saver = new DiskDataSaver<ServerAreaReport>("./reports");
            var objects_saver = new DiskDataSaver<ObjectInArea>("./objects");
            Bind<IUsersProvider>().ToConstant(new FileUsersProvider(user_saver, new DiskIndexSaver("users.index")));
            Bind<IAreaToCheckMetadataProvider>().ToConstant(new FileAreaToCheckMatadata(area_saver, new DiskIndexSaver("areas.index")));
            Bind<IObjectReportSubmissions>().ToConstant(new DiskReportSubmisions(report_saver,new DiskIndexSaver("reports.index")));
            Bind<IAreaObjectsProvider>().ToConstant(new DiskReportObjects(objects_saver, new DiskIndexSaver("objects.index")));
            Bind<ITokenProvider>().ToConstant(new MemoryTokenProvider());
        }
    }
}
