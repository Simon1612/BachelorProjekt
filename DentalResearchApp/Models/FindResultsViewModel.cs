using System.Collections.Generic;
using MongoDB.Bson;

namespace DentalResearchApp.Models
{
    public class FindResultsViewModel
    {
        public int StudyId { get; set; }
        public List<string> Studies { get; set; }
        public string SelectedStudy { get; set; }

        public ObjectId SessionId { get; set; }
        public List<string> Sessions { get; set; }
        public string SelectedSession { get; set; }

        public List<string> Surveys { get; set; }
        public string SelectedSurvey { get; set; }

        public List<int> Participants { get; set; }
        public int SelectedParticipant { get; set; }
    }
}
