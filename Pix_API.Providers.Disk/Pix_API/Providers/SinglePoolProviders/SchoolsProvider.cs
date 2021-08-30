using System.Linq;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.SinglePoolProviders
{
	public class SchoolsProvider : SinglePoolStorageProvider<School>, ISchoolProvider
	{
		public SchoolsProvider(DataSaver<School> saver)
			: base(saver)
		{
		}

		public void AddSchool(School school)
		{
			AddSingleObject(school, school.CreatorUserID);
		}

		public School GetSchool(int UserOwner_Id)
		{
			return base.storage.FirstOrDefault((School s) => s.Id == UserOwner_Id);
		}

		public void UpdateSchool(School school, int UserOwner_Id)
		{
			AddOrUpdateSingleObject(school, UserOwner_Id);
		}
	}
}
