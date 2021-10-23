using Pix_API.Providers;
using Pix_API.Providers.BaseClasses;
using System.Collections.Generic;
using System.Linq;

namespace Base.Providers.ContainersProviders
{
	public class SinglePoolStorageProvider<T>
	{
		protected DataSaver<T> saver;

		private List<IdObjectBinder<T>> storage_rw;

		protected IEnumerable<T> storage => storage_rw.Select((IdObjectBinder<T> arg) => arg.Obj);

		public SinglePoolStorageProvider(DataSaver<T> saver)
		{
			this.saver = saver;
			storage_rw = saver.LoadAll();
		}

		protected void AddOrUpdateSingleObject(T obj, int Id)
		{
			storage_rw.RemoveAll((IdObjectBinder<T> s) => s.Id == Id);
			AddSingleObject(obj, Id);
		}

		protected void AddSingleObject(T questionResult, int Id)
		{
			IdObjectBinder<T> idObjectBinder = new IdObjectBinder<T>(Id, questionResult);
			storage_rw.Add(idObjectBinder);
			saver.SaveInBackground(idObjectBinder);
		}

		protected T GetSingleObject(int Id)
		{
			IdObjectBinder<T> idObjectBinder = storage_rw.FirstOrDefault((IdObjectBinder<T> s) => s.Id == Id);
			if (idObjectBinder != null)
			{
				return idObjectBinder.Obj;
			}
			return default(T);
		}

		protected void RemoveObject(int Id)
		{
			IdObjectBinder<T> idObjectBinder = storage_rw.FirstOrDefault((IdObjectBinder<T> s) => s.Id == Id);
			if (idObjectBinder != null)
			{
				storage_rw.Remove(idObjectBinder);
				saver.Remove(Id);
			}
		}
	}
}
