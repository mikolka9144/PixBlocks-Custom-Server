using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers
{
	public class UserDatabaseProvider : SinglePoolStorageProvider<User>, IUserDatabaseProvider
	{
		private IdAssigner idAssigner;

		public UserDatabaseProvider(DataSaver<User> saver)
			: base(saver)
		{
			idAssigner = new IdAssigner(new DiskIndexSaver("users.index"));
		}

		public void AddUser(User user)
		{
			user.Id = idAssigner.NextEmptyId;
			AddSingleObject(user, user.Id.Value);
		}

		public bool ContainsUserWithEmail(string email)
		{
			return base.storage.Any((User s) => s.Email == email);
		}

		public bool ContainsUserWithLogin(string login)
		{
			return base.storage.Any((User s) => s.Student_login == login);
		}

		public List<User> GetAllUsersBelongingToClass(int classId)
		{
			return base.storage.Where((User arg) => arg.Student_studentsClassId == classId).ToList();
		}

		public User GetUser(string EmailOrLogin)
		{
			return base.storage.FirstOrDefault((User s) => s.Email == EmailOrLogin || s.Student_login == EmailOrLogin);
		}

		public User GetUser(int Id)
		{
			return GetSingleObject(Id);
		}

		public void RemoveUser(int Id)
		{
			RemoveObject(Id);
		}

		public void UpdateUser(User user)
		{
			AddOrUpdateSingleObject(user, user.Id.Value);
		}
	}
}
