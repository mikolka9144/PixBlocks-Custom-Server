using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.BaseClasses;

namespace Pix_API.Providers.ContainersProviders
{
    public abstract class MultiplePoolStorageProvider<T>:SinglePoolStorageProvider<List<T>>
    {
        public MultiplePoolStorageProvider(DataSaver<List<T>> saver) : base(saver)
        {
        }

        protected void AddOrUpdateObject(T questionResult, int Id,Func<T,T,bool> id_equalizer)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(Id);
            AllQuestionResults.RemoveAll(s => id_equalizer(s, questionResult));
            AddObject(questionResult, Id);
        }
        protected void AddObject(T questionResult, int Id)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(Id);

            AllQuestionResults.Add(questionResult);
            AddOrUpdateSingleObject(AllQuestionResults, Id);
        }
        protected void RemoveAllObjects(Predicate<T> obj,int Id)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(Id);
            AllQuestionResults.RemoveAll(obj);
            AddOrUpdateSingleObject(AllQuestionResults, Id);
        }
        protected override List<T> GetSingleObjectOrCreateNew(int Id)
        {
            var obj = base.GetSingleObjectOrCreateNew(Id);
            return obj ?? new List<T>();
        }
    }

}
