using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class BloodPressureViewModel
    {
        [ScaffoldColumn(false)]
        public int? BloodPressureId { get; set; }
        public string UserId { get; set; }
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public int Pulse { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ReadingDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ReadingDateUTC { get; set; }

    }
}