using System;
using System.Security.Cryptography;
using System.Text;
using PixBlocks.Server.DataModels.DataModels;
using Pix_API.Providers;

namespace Pix_API
{
    public static class Utills
    {
        public static void SetupUser(this User user,int id)
        {
            user.Id = id;
            user.IsEmailActivated = true;
        }
        public static string ConvertPasswordToMD5(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x3"));
            }
            return stringBuilder.ToString();
        }
        public static string CleanPixString(this string pixString)
        {
            if (pixString[0] == '\"')
            {
                return pixString.Substring(1, pixString.Length - 2);
            }
            return pixString;
        }
        public static bool MatchesPasswordWith(this AuthorizeData auth,User user)
        {
            return auth.PasswordMD5 == user.Md5Password;
        }
        public static bool IsAuthorizeValid(this IUserDatabaseProvider databaseProvider,AuthorizeData auth)
        {
            var server_user = databaseProvider.GetUser(auth.UserId);
            return auth.MatchesPasswordWith(server_user);
        }
    }
}
