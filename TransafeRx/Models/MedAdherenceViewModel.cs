using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class MedAdherenceViewModel
    {
        public string Id { get; set; }
        public string Medication { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string Status { get; set; }
        public decimal Score { get; set; }

    }
}