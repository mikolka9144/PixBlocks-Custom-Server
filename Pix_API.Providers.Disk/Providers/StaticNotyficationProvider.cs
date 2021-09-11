using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers
{
	public class StaticNotyficationProvider : INotyficationProvider
	{
		public Notification GetNotyficationForUser(string languageKey, User user)
		{
			return null;
		}
	}
}
