using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class SurescriptsViewModel
    {
        public int ID { get; set; }
        public string MRN { get; set; }
        public string DrugName { get; set; }
        public DateTime DispenseDate { get; set; }
        public int? DaysSupply { get; set; }
        public int? DispenseAmount { get; set; }
        public string Instructions { get; set; }
        
    }
}