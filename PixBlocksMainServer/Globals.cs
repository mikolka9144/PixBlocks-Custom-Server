using System;
using System.Collections.Generic;
using System.Linq;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;

namespace Pix_API.PixBlocks.MainServer
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
                return new UserAddingResult
                {
                    IsEmailExist = true
                };
            }
            user.SetupUser();
            databaseProvider.AddUser(user);
            return new UserAddingResult
            {
                User = user
            };
        }

        public UserAuthorizeResult AuthorizeUser(string loginOrEmail, string password, bool md5, LicenseData licenseData)
        {

            var potentialAnstractUser = abstractUsers.FirstOrDefault(s => s.login == loginOrEmail);
            if (potentialAnstractUser != null)
            {
                if (potentialAnstractUser.password == password)
                {
                    var user_index = abstractUsers.IndexOf(potentialAnstractUser) + 1;
                    return new UserAuthorizeResult
                    {
                        User = potentialAnstractUser.GenerateUser(-user_index),
                        IsPasswordCorrect = true,
                        PixBlocksLicense = new PixBlocksLicense(LicenseType.Free)
                    };
                }
            }
            else
            {
                User user = databaseProvider.GetUser(loginOrEmail);
                if (user != null)
                {
                    string text = Utills.ConvertPasswordToMD5(password);


                    bool flag = password == user.Student_explicitPassword && user.Student_isStudent;
                    if (user.Md5Password == text || flag)
                    {
                        user.RegisterLogin();
                        databaseProvider.UpdateUser(user);
                        return new UserAuthorizeResult
                        {
                            User = user,
                            IsPasswordCorrect = true,
                            PixBlocksLicense = new PixBlocksLicense(LicenseType.Free),
                            SchoolLogoInBase64 = brandingProvider.GetBase64LogoForUser(user.Id.Value)
                        };
                    }
                }
            }
            return new UserAuthorizeResult
            {
                IsPasswordCorrect = false
            };
        }

        public bool StudentsLoginsCheckAvaible(string studentLogin)
        {
            return !abstractUsers.Any(s => s.login == studentLogin && !databaseProvider.ContainsUserWithLogin(studentLogin));
        }
    }
}
