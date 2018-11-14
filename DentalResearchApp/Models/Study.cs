using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalResearchApp.Models
{
    public class Study
    {
        public int StudyId { get; set; }
        public string Description { get; set; }
        public string StudyName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
