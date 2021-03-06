﻿using DentalResearchApp.Code.Impl;

namespace DentalResearchApp.Code.Interfaces
{
    public interface IManagerFactory
    {
        ISurveyLinkManager CreateSurveyLinkManager();
    
        ISignupLinkManager CreateSignupLinkManager();

        ISurveyManager CreateSurveyManager();

        IUserManager CreateUserManager();

        IExternalDbManager CreateExternalDbManager();

        ISessionManager CreateSessionManager();
    }
}