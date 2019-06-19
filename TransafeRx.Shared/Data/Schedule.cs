//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransafeRx.Shared.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            this.MedicationActivities = new HashSet<MedicationActivity>();
            this.Messages = new HashSet<Message1>();
        }
    
        public int ScheduleId { get; set; }
        public string UserId { get; set; }
        public Nullable<int> UserMedicationId { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public byte DayOfWeek { get; set; }
        public System.TimeSpan ScheduleTime { get; set; }
        public Nullable<int> GroupId { get; set; }
        public bool Inactive { get; set; }
        public string Dosage { get; set; }
        public Nullable<System.DateTime> CreatedDateUTC { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDateDTO { get; set; }
        public string CreatedDateCTZ { get; set; }
        public Nullable<System.DateTime> UpdatedDateUTC { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedDateDTO { get; set; }
        public string UpdatedDateCTZ { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicationActivity> MedicationActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message1> Messages { get; set; }
        public virtual UserMedication UserMedication { get; set; }
    }
}
