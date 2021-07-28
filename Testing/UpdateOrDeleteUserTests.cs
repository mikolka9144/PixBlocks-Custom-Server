using System;
using NUnit.Framework;
using Pix_API;
using Pix_API.Providers;
using PixBlocks.Server.DataModels.DataModels;
using Newtonsoft.Json;
using System.Threading;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Interfaces;

namespace Testing
{
    [TestFixture()]
    public class UpdateOrDeleteUserTests
    {

        [Test]
        public void Normal_Case()
        {
            var bundle = Prepare();
            var user = new User() { Name = "test" };
            var parameters = new string[] { JsonConvert.SerializeObject(user)};
            var result = bundle.logic.RegisterNewUser(parameters);

            result.User.Surname = "a2";
            bundle.logic.UpdateOrDeleteUser(new string[] {
                JsonConvert.SerializeObject(result.User),
                JsonConvert.SerializeObject(new AuthorizeData(result.User))
                });
            Thread.Sleep(10);
            Assert.AreEqual(user.Name, bundle.saver.LastSavedObject.Obj.Name);
            Assert.AreEqual(result.User.Surname, bundle.saver.LastSavedObject.Obj.Surname);
        }
        [Test]
        public void Id_Change_Atempt_Case()
        {
            var bundle = Prepare();
            var user = new User() { Name = "aaa" };
            var parameters = new string[] { JsonConvert.SerializeObject(user) };
            var result = bundle.logic.RegisterNewUser(parameters);
            var server_asigned_id = result.User.Id;

            var client_user = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(result.User));

            client_user.Id = 999;
            Assert.Throws(typeof(Exception),() =>
            {
                bundle.logic.UpdateOrDeleteUser(new string[] {
                JsonConvert.SerializeObject(client_user),
                JsonConvert.SerializeObject(new AuthorizeData(client_user))
                });
            });
        }

        public (MockSaver<User> saver,MainLogicBuilder builder,Main_Logic logic) Prepare()
        {
            var saver = new MockSaver<User>();
            var builder = new MainLogicBuilder
            {
                DatabaseProvider = new UserDatabaseProvider(saver)
            };
            var Main_Logic = builder.Build();
            return (saver, builder, Main_Logic);
        }

    }
}
