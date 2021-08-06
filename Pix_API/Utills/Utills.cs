using System;
using System.Security.Cryptography;
using System.Text;
using PixBlocks.Server.DataModels.DataModels;
using Pix_API.Providers;
using PixBlocks.ServerFasade.ServerAPI;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API
{
    public static class Utills
    {
        private static ServerApi api_documentation;
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
        public static void SetupExam(this Exam exam)
        {
            exam.CreationDate = DateTime.Now;
        }
    }
}
