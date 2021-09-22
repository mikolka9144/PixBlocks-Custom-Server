using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Interfaces
{
	public interface ISchoolProvider
	{
		void AddSchool(School school);

		void UpdateSchool(School school, int UserOwner_Id);

		School GetSchool(int UserOwner_Id);
        void RemoveSchool(int userId);
    }
}
