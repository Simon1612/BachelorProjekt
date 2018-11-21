using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalResearchApp.Models
{
    public class StudyDetailsViewModel
    {
        public int StudyId { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public List<int> Participants { get; set; }
        public List<string> Sessions { get; set; }
    }
}
