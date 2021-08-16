using System;
using System.Linq;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.SinglePoolProviders
{
    public class SchoolsProvider : SinglePoolStorageProvider<School>, ISchoolProvider
    {
        public SchoolsProvider(DataSaver<School> saver) : base(saver)
        {
            var id_list = storage.Select(s => s.Obj).Select(s => s.Id).ToList();
        }

        public void AddSchool(School school)
        {
            AddSingleObject(school, school.CreatorUserID);
        }

        public School GetSchool(int UserOwner_Id)
        {
            return storage.FirstOrDefault(s => s.Id == UserOwner_Id).Obj;
        }

        public void UpdateSchool(School school, int UserOwner_Id)
        {
            AddOrUpdateSingleObject(school, UserOwner_Id);
        }
    }


}
