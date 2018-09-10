using System;

namespace DentalResearchApp.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
