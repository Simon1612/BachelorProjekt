using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalResearchApp.Models
{
    public class StudyViewModel
    {
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public IEnumerable<SelectListItem> PatientsList { get; set; }
        public IEnumerable<int> SelectedPatients { get; set; }
        public IEnumerable<SelectListItem> SessionList { get; set; }
        public IEnumerable<string> SelectedSession { get; set; }
    }
}
