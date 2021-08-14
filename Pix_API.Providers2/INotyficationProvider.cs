using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers
{
    public interface INotyficationProvider
    {
        Notification GetNotyficationForUser(string languageKey, User user);
    }
}
