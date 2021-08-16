using System;
using System.Collections.Generic;
using System.IO;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<Countrie> GetAllCountries(object _)
        {
            return countriesProvider.GetAllCountries();
        }

        public UserAddingResult RegisterNewUser(User user)
        {
            if (user.Email != null && databaseProvider.ContainsUserWithEmail(user.Email))
            {
                return new UserAddingResult() { IsEmailExist = true };
            }
            user.CreationDate = DateTime.Now;
            databaseProvider.AddUser(user);
            return new UserAddingResult() { User = user };
        }
        public UserAuthorizeResult AuthorizeUser(string loginOrEmail, string password, bool md5, LicenseData licenseData)
        {
            var user_in_question = databaseProvider.GetUser(loginOrEmail);
            if (user_in_question != null)
            {
                var Md5Password = Utills.ConvertPasswordToMD5(password);
                var IsStudentLoginValid = password == user_in_question.Student_explicitPassword && user_in_question.Student_isStudent == true;
                if (user_in_question.Md5Password == Md5Password || IsStudentLoginValid)
                {
                    user_in_question.RegisterLogin();
                    databaseProvider.UpdateUser(user_in_question);

                    return new UserAuthorizeResult()
                    {
                        User = user_in_question,
                        IsPasswordCorrect = true,
                        PixBlocksLicense = new PixBlocksLicense(LicenseType.Free),
                        SchoolLogoInBase64 = brandingProvider.GetBase64LogoForUser(user_in_question.Id.Value)
                    };
                }
            }
            return new UserAuthorizeResult() { IsPasswordCorrect = false };
        }
        public bool StudentsLoginsCheckAvaible(string studentLogin)
        {
            return !databaseProvider.ContainsUserWithLogin(studentLogin);
        }
        public AccountActivationStatus IsParentEmailActivated(int childID)
        {
            return new AccountActivationStatus() { IsEmailActivated = true };
        }
    }
}
