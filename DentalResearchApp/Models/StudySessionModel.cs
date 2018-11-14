using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class StudySessionModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SessionName { get; set; }
        public string StudyId { get; set; }
        public IEnumerable<string> SelectedParticipants { get; set; }
        public IEnumerable<SelectListItem> AllParticipants { get; set; }
        public IEnumerable<string> SelectedSurveys { get; set; }
        public IEnumerable<SelectListItem> AllSurveys { get; set; }
    }
}
