using System;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class UserMedicationViewModel
    {
        [ScaffoldColumn(false)]
        public int UserMedicationId { get; set; }
        public string UserId { get; set; }
        public string DrugName { get; set; }
        public string Route { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Quantity { get; set; }
    }
}