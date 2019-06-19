using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace TransafeRx.Models
{
    public class ExternalDrugViewModel
    {
        public string AlphaId { get; set; }
        public string UserId { get; set; }
        public string DrugName { get; set; }
        public DateTime StartDate { get; set; }
        public string Route { get; set; }
        public string Instructions { get; set; }
        public string Source { get; set; }
        public int? DaysSupply { get; set; }

    }
}