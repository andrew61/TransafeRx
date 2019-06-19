using System;
using EntityFrameworkExtras.EF6;

namespace TransafeRx.Shared.Models
{
    [UserDefinedTableType("ScheduleType")]
    public class ScheduleType
    {
        [UserDefinedTableTypeColumn(1)]
        public int ScheduleId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public string UserId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public int? UserMedicationId { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public DateTime StartDate { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public Nullable<DateTime> EndDate { get; set; }

        [UserDefinedTableTypeColumn(6)]
        public TimeSpan ScheduleTime { get; set; }

        [UserDefinedTableTypeColumn(7)]
        public int DayOfWeek { get; set; }

        [UserDefinedTableTypeColumn(8)]
        public Nullable<int> GroupId { get; set; }

        [UserDefinedTableTypeColumn(9)]
        public bool Inactive { get; set; }

        [UserDefinedTableTypeColumn(10)]
        public float Dosage { get; set; }

        [UserDefinedTableTypeColumn(11)]
        public Nullable<DateTime> CreatedDateUTC { get; set; }

        [UserDefinedTableTypeColumn(12)]
        public Nullable<DateTimeOffset> CreatedDateDTO { get; set; }

        [UserDefinedTableTypeColumn(13)]
        public string CreatedDateCTZ { get; set; }

        [UserDefinedTableTypeColumn(14)]
        public Nullable<DateTime> UpdatedDateUTC { get; set; }

        [UserDefinedTableTypeColumn(15)]
        public Nullable<DateTimeOffset> UpdatedDateDTO { get; set; }

        [UserDefinedTableTypeColumn(16)]
        public string UpdatedDateCTZ { get; set; }
    }
}
