using System;
using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers
{
    public class StaticNotyficationProvider:INotyficationProvider
    {

        public Notification GetNotyficationForUser(string languageKey, User user)
        {
            return new Notification()
                {
                    IsActive = true,
                    NotyficationText = "Elo wdwdwdwdwdwd",
                    ImageInBase64 = "iVBORw0KGgoAAAANSUhEUgAAABwAAAAaCAYAAACkVDyJAAAACXBIWXMAAA3XAAAN1wFCKJt4AAAA2klEQVRIicWWQRLDIAhFwem9gycnC0fHKC" +
                        "hoYv8m0xp48KFpkIAYFkV8pStGcwwCcwF6AmugJza4CC8o1JXVFVvkdQTgDx3+AFKl3u48qnN3Hb4JJr66fAW4M0spRspBGJOlanULS6GBsoJ2YAYMnCGMXc5+hpvWSnlqofZosz" +
                        "5FZha296gzbAMk6MgB7UztUArM0FWrh1s6g7eJtPPp0swCrDBpQwEmM7RY57V3+MN/fHYujRlomcNOEQ+gpatdFeDK68KKzr9itF982V0BfvlvLwJPKpxalqwbFUdzOSHPZqUAAAAASUVORK5CYII="

                };
        }
    }
}
