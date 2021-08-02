using System;
using System.Collections.Generic;
using System.IO;
using Pix_API.Providers.ContainersProviders;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace Pix_API.Providers
{
    public class DiskDataSaver<T>:DataSaver<T>
    {
        private readonly string path;

        public DiskDataSaver(string path)
        {
            this.path = path;
        }

        public override List<IdObjectBinder<T>> LoadAll()
        {
            var list = new List<IdObjectBinder<T>>();
            var files = GetFilesFromDir();
            foreach (var item in files)
            {
                var user_id_str = Path.GetFileName(item);
                var t = JsonConvert.DeserializeObject<T>(File.ReadAllText(item));
                list.Add(new IdObjectBinder<T>(Convert.ToInt32(user_id_str),t));
            }
            return list;
        }

        private string[] GetFilesFromDir()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Directory.GetFiles(path);
        }

        public override void Save(IdObjectBinder<T> obj)
        {
            var file_path = Path.Combine(path, Convert.ToString(obj.Id));
            var json_content = JsonConvert.SerializeObject(obj.Obj);
            File.WriteAllText(file_path, json_content);
        }

    }

    public abstract class DataSaver<T>
    {
        public abstract void Save(IdObjectBinder<T> obj);
        public abstract List<IdObjectBinder<T>> LoadAll();
        public void SaveInBackground(IdObjectBinder<T> obj)
        {
            Action p = () => Save(obj);
            new Task(p).Start();
        }
    }
}
