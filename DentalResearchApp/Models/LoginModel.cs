﻿namespace DentalResearchApp.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserCredentials UserCredentials
        {
            get => default(UserCredentials);
            set
            {
            }
        }
    }
}
