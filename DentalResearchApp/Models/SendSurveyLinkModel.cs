using MongoDB.Bson;

namespace DentalResearchApp.Models
{
    public class SendSurveyLinkModel
    {
        public int ParticipantId { get; set; }
        public string ParticipantEmail { get; set; }
        public ObjectId UserSessionId { get; set; }
        public string SurveyName { get; set; }
    }
}
