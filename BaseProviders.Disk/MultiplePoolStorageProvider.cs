using System;
using System.Collections.Generic;

namespace Pix_API.Base.Disk
{
	public abstract class MultiplePoolStorageProvider<T> : SinglePoolStorageProvider<List<T>>
	{
		public MultiplePoolStorageProvider(DataSaver<List<T>> saver)
			: base(saver)
		{
		}

		protected void AddOrUpdateObject(T questionResult, int Id, Func<T, T, bool> id_equalizer)
		{
			GetObjectOrCreateNew(Id).RemoveAll((T s) => id_equalizer(s, questionResult));
			AddObject(questionResult, Id);
		}

		protected void AddObject(T questionResult, int Id)
		{
			List<T> objectOrCreateNew = GetObjectOrCreateNew(Id);
			objectOrCreateNew.Add(questionResult);
			AddOrUpdateSingleObject(objectOrCreateNew, Id);
		}

		protected void RemoveAllObjects(Predicate<T> obj, int Id)
		{
			List<T> objectOrCreateNew = GetObjectOrCreateNew(Id);
			objectOrCreateNew.RemoveAll(obj);
			AddOrUpdateSingleObject(objectOrCreateNew, Id);
		}

		protected List<T> GetObjectOrCreateNew(int Id)
		{
			return GetSingleObject(Id) ?? new List<T>();
		}
	}
}
