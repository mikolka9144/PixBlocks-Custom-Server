using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Interfaces
{
	public interface IUserDatabaseProvider
	{
		User GetUser(string EmailOrLogin);

		User GetUser(int Id);

		void RemoveUser(int Id);

		void AddUser(User user);

		void UpdateUser(User user);

		bool ContainsUserWithEmail(string email);

		bool ContainsUserWithLogin(string login);

		List<User> GetAllUsersBelongingToClass(int classId);
        List<User> GetAllUsersInCountry(int value);
    }
}
