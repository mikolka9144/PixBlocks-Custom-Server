using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : MultiplePoolStorageProvider<StudentsClass>, IStudentClassProvider
	{
		private readonly IdAssigner idAssigner;

		private readonly IUserDatabaseProvider userDatabase;

		public StudentClassesProvider(DataSaver<List<StudentsClass>> saver, IUserDatabaseProvider userDatabase)
			: base(saver)
		{
			idAssigner = new IdAssigner(new DiskIndexSaver("classes.index"));
			this.userDatabase = userDatabase;
		}

		public void AddClassForUser(StudentsClass studentsClass, int userId)
		{
			studentsClass.Id = idAssigner.NextEmptyId;
			AddObject(studentsClass, userId);
		}

		public void EditClassForUser(StudentsClass studentsClass, int userId)
		{
			AddOrUpdateObject(studentsClass, userId, (StudentsClass a, StudentsClass b) => a.Id == b.Id);
		}

		public List<StudentsClass> GetClassesForUser(int Id)
		{
			return GetObjectOrCreateNew(Id);
		}

		public StudentsClass GetStudentsClassByGlobalId(int classId)
		{
			return base.storage.SelectMany((List<StudentsClass> s) => s).FirstOrDefault((StudentsClass s) => s.Id == classId);
		}

		public StudentsClass GetStudentsClassById(int userId, int classId)
		{
			return GetObjectOrCreateNew(userId).FirstOrDefault((StudentsClass s) => s.Id == classId);
		}

		public List<User> GetStudentsInClassForUser(int userId, int classId)
		{
			if (!GetObjectOrCreateNew(userId).Any((StudentsClass arg) => arg.Id.Value == classId))
			{
				return null;
			}
			return userDatabase.GetAllUsersBelongingToClass(classId);
		}

		public void RemoveClass(StudentsClass studentsClass)
		{
			GetObjectOrCreateNew(studentsClass.TeacherID).RemoveAll((StudentsClass obj) => obj.Id == studentsClass.Id);
		}
	}
}
