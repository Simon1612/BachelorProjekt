using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalResearchApp.Models
{
    public class StudySessionViewModel
    {
        public string SessionName { get; set; }
        public int StudyId { get; set; }
        public IEnumerable<string> SelectedSurveys { get; set; }
        public IEnumerable<SelectListItem> AllSurveys { get; set; }
    }
}
