using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class SurveyQuestionLogicViewModel
    {
        public int LogicId { get; set; }
        public int QuestionId { get; set; }
        public string Expression { get; set; }
        public string Components { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [UIHint("SurveyQuestionLogicActions")]
        public SurveyQuestionLogicActionViewModel LogicAction { get; set; }

        [UIHint("SurveyLogicActionQuestions")]
        public SurveyLogicActionQuestionViewModel LogicActionQuestion { get; set; }
    }
}