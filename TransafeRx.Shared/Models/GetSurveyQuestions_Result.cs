using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransafeRx.Shared.Data
{
    [NotMapped]
    public partial class GetSurveyQuestions_Result
    {
        public bool IsFirstQuestion { get; set; }
        public GetSurveyAnswers_Result Answer { get; set; }
        public List<GetSurveyAnswers_Result> Answers { get; set; }
        public List<GetSurveyQuestionOptions_Result> Options { get; set; }
    }
}