using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.API.Models
{
    public class ChartModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? Aggregate { get; set; }
    }
}