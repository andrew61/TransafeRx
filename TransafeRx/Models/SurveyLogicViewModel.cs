using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class SurveyLogicViewModel
    {
        public int LogicId { get; set; }
        public int SurveyId { get; set; }
        public string Expression { get; set; }

        public string Components { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [UIHint("SurveyLogicActions")]
        public SurveyLogicActionViewModel LogicAction { get; set; }
    }
}