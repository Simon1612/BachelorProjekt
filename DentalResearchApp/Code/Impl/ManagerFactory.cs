﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Driver;

namespace DentalResearchApp.Code.Impl
{
    public class ManagerFactory : IManagerFactory 
    {
        private readonly IMongoClient _client;
        private readonly string _linkDbName;
        private readonly string _surveyDbName;
        private readonly string _userDbName;


        public ManagerFactory(string connectionString, string linkDbName, string surveyDbName, string userDbName)
        {
            _client = new MongoClient(connectionString);
            _linkDbName = linkDbName;
            _surveyDbName = surveyDbName;
            _userDbName = userDbName;
        }

        public ILinkManager CreateLinkManager()
        {
            return new LinkManager(_client, _linkDbName);
        }

        public ISurveyManager CreateSurveyManager()
        {
            return new SurveyManager(_client, _surveyDbName);
        }

        public IUserManager CreateUserManager()
        {
            return new UserManager(_client, _userDbName);
        }
    }
}