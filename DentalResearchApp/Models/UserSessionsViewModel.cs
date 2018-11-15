using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalResearchApp.Models
{
    public class UserSessionsViewModel
    {
        public int ParticipantId { get; set; }
        public string StudyName { get; set; }
        public List<UserSessionResult> UserSessions { get; set; }
    }

    public class UserSessionResult
    {
        public string SessionId { get; set; }
    }
}
