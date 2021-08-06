using System;
namespace Pix_API.Providers.BaseClasses
{
    public class IdObjectBinder<T>
    {
        public IdObjectBinder(int Id, T obj)
        {
            this.Id = Id;
            Obj = obj;
        }

        public int Id { get; }
        public T Obj { get; }
    }
}
