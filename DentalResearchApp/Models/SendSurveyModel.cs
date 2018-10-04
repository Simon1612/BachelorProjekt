using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalResearchApp.Models
{
    public class SendSurveyModel
    {
        public List<string> Studies { get; set; }
        public List<string> Patients { get; set; }
        public List<string> Survey { get; set; }
    }
}
