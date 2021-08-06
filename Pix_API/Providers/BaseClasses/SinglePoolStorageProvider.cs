using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.ContainersProviders;

namespace Pix_API.Providers.BaseClasses
{
    public class SinglePoolStorageProvider<T>
    {
        protected DataSaver<T> saver;
        protected List<IdObjectBinder<T>> storage;

        public SinglePoolStorageProvider(DataSaver<T> saver)
        {
            this.saver = saver;
            storage = saver.LoadAll();
        }
        protected void AddOrUpdateObject(T obj, int Id)
        {
            storage.RemoveAll(s => s.Id == Id);
            AddObject(obj, Id);

        }
        protected void AddObject(T questionResult, int Id)
        {
            var obj = new IdObjectBinder<T>(Id, questionResult);
            storage.Add(obj);
            saver.SaveInBackground(obj);
        }
        protected virtual T GetObjectOrCreateNew(int Id)
        {
            var user_results = storage.FirstOrDefault(s => s.Id == Id);
            if (user_results == null)
            {
                var newUserResults = new IdObjectBinder<T>(Id,default(T));
                storage.Add(newUserResults);
                return newUserResults.Obj;
            }
            return user_results.Obj;
        }
    }
}
