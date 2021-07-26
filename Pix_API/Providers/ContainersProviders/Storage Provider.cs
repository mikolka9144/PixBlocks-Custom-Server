using System;
using System.Collections.Generic;
using System.Linq;

namespace Pix_API.Providers.ContainersProviders
{
    public abstract class Storage_Provider<T>
    {
        public Storage_Provider(DataSaver<List<T>> saver)
        {
            this.saver = saver;
            storage = saver.LoadAll();
        }
        protected List<IdObjectBinder<List<T>>> storage;
        private readonly DataSaver<List<T>> saver;

        protected void AddOrUpdateObject(T questionResult, int Id,Func<T,T,bool> id_equalizer)
        {
            var AllQuestionResults = GetUserQuestionResultsOrCreateNew(Id);
            if (AllQuestionResults.Obj.Any(s => id_equalizer(s, questionResult)))
            {
                AllQuestionResults.Obj.RemoveAll(s => id_equalizer(s, questionResult));
            }
            AllQuestionResults.Obj.Add(questionResult);
            saver.SaveInBackground(AllQuestionResults);
        }

        protected List<T> GetAllObjectsForUser(int Id)
        {
            return GetUserQuestionResultsOrCreateNew(Id).Obj;
        }
        private IdObjectBinder<List<T>> GetUserQuestionResultsOrCreateNew(int Id)
        {
            var user_results = storage.FirstOrDefault(s => s.Id == Id);
            if (user_results == null)
            {
                var newUserResults = new IdObjectBinder<List<T>>(Id, new List<T>());
                storage.Add(newUserResults);
                return newUserResults;
            }
            return user_results;
        }
    }
    public class IdObjectBinder<T>
    {
        public IdObjectBinder(int Id,T obj)
        {
            this.Id = Id;
            Obj = obj;
        }

        public int Id { get; }
        public T Obj { get; }
    }
}
