using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class GlucoseViewModel
    {
        public int GlucoseId { get; set; }
        public string UserId { get; set; }
        public int GlucoseLevel { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReadingDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReadingDateUTC { get; set; }
    }
}