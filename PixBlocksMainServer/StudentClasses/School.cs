using System;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public School AddSchool(School school, AuthorizeData authorize)
        {
            school.CreatorUserID = authorize.UserId;
            schoolProvider.AddSchool(school);
            return school;
        }

        public School GetSchool(User user, AuthorizeData authorize)
        {
            return schoolProvider.GetSchool(authorize.UserId);
        }

        public School UpdateOrDeleteSchool(School school, AuthorizeData authorize)
        {
            schoolProvider.UpdateSchool(school, authorize.UserId);
            return school;
        }
    }
}
