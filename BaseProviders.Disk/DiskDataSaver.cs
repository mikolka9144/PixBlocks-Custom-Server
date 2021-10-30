using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Pix_API.Base.Utills;

namespace Pix_API.Base.Disk
{
	public class DiskDataSaver<T> : DataSaver<T>
	{
		private readonly string path;

		public DiskDataSaver(string path)
		{
			this.path = path;
		}

		public override List<IdObjectBinder<T>> LoadAll()
		{
			List<IdObjectBinder<T>> list = new List<IdObjectBinder<T>>();
			string[] filesFromDir = GetFilesFromDir();
			foreach (string obj in filesFromDir)
			{
				string fileName = Path.GetFileName(obj);
				T obj2 = JsonConvert.DeserializeObject<T>(File.ReadAllText(obj));
				list.Add(new IdObjectBinder<T>(Convert.ToInt32(fileName), obj2));
			}
			return list;
		}

		private string[] GetFilesFromDir()
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return Directory.GetFiles(path);
		}

		public override void Save(IdObjectBinder<T> obj)
		{
			string text = Path.Combine(path, Convert.ToString(obj.Id));
			string contents = JsonConvert.SerializeObject(obj.Obj);
			File.WriteAllText(text, contents);
		}

		public override void Remove(int Id)
		{
			File.Delete(Path.Combine(path, Convert.ToString(Id)));
		}
	}
}
