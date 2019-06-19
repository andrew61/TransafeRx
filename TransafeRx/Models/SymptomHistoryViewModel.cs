using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class SymptomHistoryViewModel
    {
        public int? SessionId { get; set; }
        public int? Score { get; set; }
        public DateTime? SurveyDate { get; set; }
    }
}