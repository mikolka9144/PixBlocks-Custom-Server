using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Interfaces
{
	public interface IStudentClassProvider
	{
		List<StudentsClass> GetClassesForUser(int Id);

		StudentsClass GetStudentsClassById(int userId, int classId);

		StudentsClass GetStudentsClassByGlobalId(int classId);

		void AddClassForUser(StudentsClass studentsClass, int userId);

		void EditClassForUser(StudentsClass studentsClass, int userId);

		void RemoveClass(StudentsClass studentsClass);

		List<User> GetStudentsInClassForUser(int userID, int classID);
	}
}
