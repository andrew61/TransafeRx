using EntityFrameworkExtras.EF6;
using System;

namespace TransafeRx.Shared.Models
{
    [UserDefinedTableType("MedicationActivityType")]
    public class MedicationActivityType
    {
        [UserDefinedTableTypeColumn(1)]
        public int ActivityTypeId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public int UserMedicationId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public Nullable<int> ScheduleId { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public Nullable<DateTime> ScheduleDate { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public DateTime ActivityUTC { get; set; }

        [UserDefinedTableTypeColumn(6)]
        public DateTimeOffset ActivityDTO { get; set; }

        [UserDefinedTableTypeColumn(7)]
        public string ActivityCTZ { get; set; }
    }
}