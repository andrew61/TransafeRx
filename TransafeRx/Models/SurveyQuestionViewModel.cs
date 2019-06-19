using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TransafeRx.Models
{
    public class SurveyQuestionViewModel
    {
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }

        [Required(ErrorMessage = "Question Type is required")]
        [UIHint("SurveyQuestionTypes")]
        public SurveyQuestionTypeViewModel QuestionType { get; set; }

        public int QuestionTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string QuestionText { get; set; }

        public string QuestionImage { get; set; }

        public int QuestionOrder { get; set; }

        public string OptionImage { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public List<SurveyQuestionOptionViewModel> Options { get; set; }

        public bool Required { get; set; }

        public SurveyQuestionViewModel()
        {
            this.Options = new List<SurveyQuestionOptionViewModel>();
        }
    }
}