using System.Collections.Generic;
using System.Threading.Tasks;
using Pix_API.Providers.BaseClasses;

namespace Pix_API.Providers
{
	public abstract class DataSaver<T>
	{
		public abstract void Save(IdObjectBinder<T> obj);

		public abstract List<IdObjectBinder<T>> LoadAll();

		public abstract void Remove(int Id);

		public void SaveInBackground(IdObjectBinder<T> obj)
		{
			new Task(delegate
			{
				Save(obj);
			}).Start();
		}
	}
}
