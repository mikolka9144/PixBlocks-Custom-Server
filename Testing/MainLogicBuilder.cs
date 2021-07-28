using System;
using System.Collections.Generic;
using Pix_API;
using Pix_API.Providers;
using Pix_API.Providers.ContainersProviders;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Testing
{
    public class MainLogicBuilder
    {
        public IQuestionResultsProvider questionResultsProvider;
        public IQuestionEditsProvider questionEditsProvider;

        public ICountriesProvider CountriesProvider;
        public IUserDatabaseProvider DatabaseProvider;

        public MainLogicBuilder()
        {
            CountriesProvider = new CountriesProvider();
            DatabaseProvider = new UserDatabaseProvider(new MockSaver<User>());
            questionResultsProvider = new QuestionResultProvider(new MockSaver<List<QuestionResult>>());
            questionEditsProvider = new QuestionEditsProvider(new MockSaver<List<EditedQuestionCode>>());
        }
        public Main_Logic Build()
        {
            return new Main_Logic(CountriesProvider, DatabaseProvider, questionResultsProvider, questionEditsProvider);
        }
    }
    public class MockSaver<T> : DataSaver<T>
    {

        public IdObjectBinder<T> LastSavedObject { get; private set; }

        public override List<IdObjectBinder<T>> LoadAll()
        {
            return new List<IdObjectBinder<T>>();
        }

        public override void Save(IdObjectBinder<T> obj)
        {
            LastSavedObject = obj;
        }
    }
}
