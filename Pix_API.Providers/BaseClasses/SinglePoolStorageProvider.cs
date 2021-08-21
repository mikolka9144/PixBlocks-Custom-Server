using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.ContainersProviders;

namespace Pix_API.Providers.BaseClasses
{
    public class SinglePoolStorageProvider<T>
    {
        protected DataSaver<T> saver;
        private List<IdObjectBinder<T>> storage_rw;
        protected IEnumerable<T> storage { get => storage_rw.Select((arg) => arg.Obj); }

        public SinglePoolStorageProvider(DataSaver<T> saver)
        {
            this.saver = saver;
            storage_rw = saver.LoadAll();
        }
        protected void AddOrUpdateSingleObject(T obj, int Id)
        {
            storage_rw.RemoveAll(s => s.Id == Id);
            AddSingleObject(obj, Id);

        }
        protected void AddSingleObject(T questionResult, int Id)
        {
            var obj = new IdObjectBinder<T>(Id, questionResult);
            storage_rw.Add(obj);
            saver.SaveInBackground(obj);
        }
        protected T GetSingleObject(int Id)
        {
            var id_obj = storage_rw.FirstOrDefault(s => s.Id == Id);
            if (id_obj != null) return id_obj.Obj;
            return default(T);
        }
    }
}
