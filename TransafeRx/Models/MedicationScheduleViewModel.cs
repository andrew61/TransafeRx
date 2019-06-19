using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace TransafeRx.Models
{
    public class MedicationScheduleViewModel
    {
        public int UserMedicationId { get; set; }
        public string UserId { get; set; }
        public string DrugName { get; set; }
        
    }
}