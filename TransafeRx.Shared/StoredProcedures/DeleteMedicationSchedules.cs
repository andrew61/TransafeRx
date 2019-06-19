using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using System.Collections.Generic;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("DeleteMedicationSchedules")]
    public class DeleteMedicationSchedules
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<ScheduleType> Schedules { get; set; }
    }
}