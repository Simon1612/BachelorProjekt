using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DentalResearchApp.Models
{
    public class UserSessionDetailsViewModel
    {
        public string StudyName { get; set; }
        public int ParticipantId { get; set; }
        public string SessionName { get; set; }
        public List<ResultLink> ResultLinks { get; set; }
    }

    public class ResultLink
    {
        public string SurveyName { get; set; }
        public ObjectId UserSessionId { get; set; }
    }
}
