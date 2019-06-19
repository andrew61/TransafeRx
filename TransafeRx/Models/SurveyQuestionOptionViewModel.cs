using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TransafeRx.Shared.Models;

namespace TransafeRx.Models
{
    public class SurveyQuestionOptionViewModel
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Option is required")]
        public string OptionText { get; set; }

        public string OptionImage { get; set; }

        public int? OptionValue { get; set; }

        public int OptionOrder { get; set; }

        public ShapeType? ShapeType { get; set; }

        public string ShapeTypeString
        {
            get { return Enum.GetName(typeof(ShapeType), this.ShapeType.GetValueOrDefault()); }
        }

        public int? ShapeTypeValue
        {
            get { return this.ShapeType.HasValue ? (int)this.ShapeType : (int?)null; }
        }

        public string Coordinates { get; set; }

        [AllowHtml]
        public string Feedback { get; set; }
    }
}