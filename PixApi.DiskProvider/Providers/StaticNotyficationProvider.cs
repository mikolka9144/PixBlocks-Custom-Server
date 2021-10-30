
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Disk
{
	public class StaticNotyficationProvider : INotyficationProvider
	{
		public Notification GetNotyficationForUser(string languageKey, User user)
		{
			return null;
		}
	}
}
