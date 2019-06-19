using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransafeRx.Shared.Models
{
    public class TimeZoneAdjustedDateInfo
    {
        public DateTime DateUTC { get; set; }
        public DateTimeOffset DateDTO { get; set; }
        public string DateCTZ { get; set; }
    }
}
