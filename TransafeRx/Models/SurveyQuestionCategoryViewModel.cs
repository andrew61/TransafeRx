using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class SurveyQuestionCategoryViewModel
    {
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int CategoryOrder { get; set; }
    }
}