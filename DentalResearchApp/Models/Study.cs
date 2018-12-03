using System;

namespace DentalResearchApp.Models
{
    public class Study
    {
        public int StudyId { get; set; }
        public string Description { get; set; }
        public string StudyName { get; set; }
        public DateTime DateCreated { get; set; }

        public ParticipantSessionModel ParticipantSessionModel
        {
            get => default(ParticipantSessionModel);
            set
            {
            }
        }
    }
}
