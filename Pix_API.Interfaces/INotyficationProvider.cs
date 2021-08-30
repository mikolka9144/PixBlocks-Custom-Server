using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Interfaces
{
	public interface INotyficationProvider
	{
		Notification GetNotyficationForUser(string languageKey, User user);
	}
}
