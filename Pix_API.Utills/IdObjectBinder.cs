namespace Pix_API.Providers.BaseClasses
{
	public class IdObjectBinder<T>
	{
		public int Id { get; }

		public T Obj { get; }

		public IdObjectBinder(int Id, T obj)
		{
			this.Id = Id;
			Obj = obj;
		}
	}
}
