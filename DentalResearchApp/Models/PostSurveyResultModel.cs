namespace DentalResearchApp.Models
{
    public class PostSurveyResultModel
    {
        public string PostId { get; set; }
        public string SurveyResult { get; set; }

        public SurveyResult SurveyResult1
        {
            get => default(SurveyResult);
            set
            {
            }
        }
    }

}
