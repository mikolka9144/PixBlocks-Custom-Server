﻿using System;
using System.Collections.Generic;
using Pix_API.Base.Utills;
using Pix_API.PixBlocks.Interfaces;

namespace Pix_API.PixBlocks.MainServer
{
    public partial class Main_Logic:ICommandRepository
    {
        private readonly ICountriesProvider countriesProvider;
        private readonly IUserDatabaseProvider databaseProvider;
        private readonly IQuestionResultsProvider questionResultsProvider;
        private readonly IQuestionEditsProvider questionEditsProvider;
        private readonly IToyShopProvider toyShopProvider;
        private readonly INotyficationProvider notyficationProvider;
        private readonly IChampionshipsMetadataProvider championshipsProvider;
        private readonly IStudentClassProvider studentClassProvider;
        private readonly IStudentClassExamsProvider studentClassExamsProvider;
        private readonly IUserCommentsProvider userCommentsProvider;
        private readonly ISchoolProvider schoolProvider;
        private readonly IBrandingProvider brandingProvider;
        private readonly IParentInfoHolder parentInfoProvider;
        private readonly ServerUtills serverUtills;
        private readonly List<IAbstractUser> abstractUsers;

        public Main_Logic(ICountriesProvider countriesProvider, IUserDatabaseProvider databaseProvider, IQuestionResultsProvider questionResultsProvider, IQuestionEditsProvider questionEditsProvider, IToyShopProvider toyShopProvider, INotyficationProvider notyficationProvider, IChampionshipsMetadataProvider championshipsProvider, IStudentClassProvider studentClassProvider, IStudentClassExamsProvider studentClassExamsProvider, IUserCommentsProvider userCommentsProvider, ISchoolProvider schoolProvider, IBrandingProvider brandingProvider, IParentInfoHolder parentInfoProvider,ServerUtills security,List<IAbstractUser> abstractUsers)
        {
            this.countriesProvider = countriesProvider;
            this.databaseProvider = databaseProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.questionEditsProvider = questionEditsProvider;
            this.toyShopProvider = toyShopProvider;
            this.notyficationProvider = notyficationProvider;
            this.championshipsProvider = championshipsProvider;
            this.studentClassProvider = studentClassProvider;
            this.studentClassExamsProvider = studentClassExamsProvider;
            this.userCommentsProvider = userCommentsProvider;
            this.schoolProvider = schoolProvider;
            this.brandingProvider = brandingProvider;
            this.parentInfoProvider = parentInfoProvider;
            this.serverUtills = security;
            this.abstractUsers = abstractUsers;
        }
    }
}
