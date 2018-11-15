using System.Collections.Generic;

namespace DentalResearchApp.Models
{
    public class InviteUserViewModel
    {
        public string Email { get; set; }
        public List<UserModel> UserList { get; set; }
    }
}
