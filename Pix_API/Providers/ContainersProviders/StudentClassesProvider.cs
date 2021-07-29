using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : Storage_Provider<StudentsClass>, IStudentClassProvider
    {
        public StudentClassesProvider(DataSaver<List<StudentsClass>> saver) : base(saver)
        {
        }

        public List<StudentsClass> GetClassesForUser(int Id)
        {
            return GetAllObjectsForUser(Id);
        }
    }

    public interface IStudentClassProvider
    {
        List<StudentsClass> GetClassesForUser(int Id);
    }
}
