using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class SymptomViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public int OptionValue { get; set; }
        public DateTime? AnswerDateUTC { get; set; }
    }
}