using System;
using System.Collections.Generic;
using System.Text;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace UnitTests
{
    public class ManagerFactoryTests
    {
        private IManagerFactory _factory;

        [Fact]
        public void ManagerFactory_CreateUserManager_UserManagerCreated()
        {
            var client = Substitute.For<IMongoClient>();

            _factory = new ManagerFactory(client, "linkDbName", "surveyDbName", "userDbName", "sessionDbName");

            var manager = _factory.CreateUserManager();

            Assert.NotNull(manager);
            Assert.IsAssignableFrom<IUserManager>(manager);
            Assert.IsType<UserManager>(manager);
        }

        [Fact]
        public void ManagerFactory_CreateSurveyManager_SurveyManagerCreated()
        {
            var client = Substitute.For<IMongoClient>();

            _factory = new ManagerFactory(client, "linkDbName", "surveyDbName", "userDbName", "sessionDbName");

            var manager = _factory.CreateSurveyManager();

            Assert.NotNull(manager);
            Assert.IsAssignableFrom<ISurveyManager>(manager);
            Assert.IsType<SurveyManager>(manager);
        }

        [Fact]
        public void ManagerFactory_CreateLinkManager_LinkManagerCreated()
        {
            var client = Substitute.For<IMongoClient>();

            _factory = new ManagerFactory(client, "linkDbName", "surveyDbName", "userDbName", "sessionDbName");

            var manager = _factory.CreateSurveyLinkManager();

            Assert.NotNull(manager);
            Assert.IsAssignableFrom<ISurveyLinkManager>(manager);
            Assert.IsType<SurveyLinkManager>(manager);
        }
    }
}
