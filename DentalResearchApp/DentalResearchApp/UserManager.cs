using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp
{
    public static class UserManager
    {
        public static UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossdi@omain.com", Role = Role.Administrator };
            }
            return user;
        }
    }
}
