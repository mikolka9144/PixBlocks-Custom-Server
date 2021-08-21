using System;
using System.Security.Cryptography;
using System.Text;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;

namespace Pix_API
{
    public static class Utills
    {
        public static void SetupUser(this User user)
        {
            user.IsEmailActivated = true;
            user.CreationDate = DateTime.Now;
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
        public static bool IsExamCreatedByUser(this IStudentClassProvider classProvider, Exam exam, int UserId)
        {
            var class_id = exam.StudentsClassId;
            return classProvider.IsClassBelongsToUser(UserId, class_id);
        }
        public static bool IsClassBelongsToUser(this IStudentClassProvider classProvider, int userId, int classId)
        {
            return classProvider.GetClassesForUser(userId).Any(s => s.Id == classId);
        }
        public static void RegisterLogin(this User user)
        {
            user.LoginsCounter++;
            user.LastLoginDate = DateTime.Now;
        }
        public static void set_Id(this ServerExam exam,int Id)
        {
            exam.Id = Id;
            exam.Exam_metadata.Id = Id;
        }
    }
}
