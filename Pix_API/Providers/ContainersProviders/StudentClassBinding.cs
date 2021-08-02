using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.ContainersProviders;

namespace Pix_API.Providers
{
    public class ClassBind
    {
        public ClassBind()
        {

        }
        public ClassBind(int classId)
        {
            this.classId = classId;
            students_ids = new List<int>();
        }
        public int classId { get; set; }
        public List<int> students_ids { get; set; }
    }
    public class StudentClassBinding:Storage_Provider<ClassBind>, IStudentClassBinding
    {

        public StudentClassBinding(DataSaver<List<ClassBind>> saver):base(saver)
        {
        }

        public void Add(int userId, int classId)
        {
            var classBind = new ClassBind( classId);
            AddObject(classBind, userId);
        }

        public ClassBind Get(int userId, int classId)
        {
            return GetUserObjectOrCreateNew(userId).Obj.Find(s => s.classId == classId);
        }

        public void Remove(int userId, int classId)
        {
            RemoveAllObjects(s => s.classId == classId, userId);
        }
    }

    public interface IStudentClassBinding
    {
        ClassBind Get(int userId, int classId);
        void Add(int userId, int classId);
        void Remove(int userId, int classId);
    }
}
