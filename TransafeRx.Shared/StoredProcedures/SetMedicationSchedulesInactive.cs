using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using System.Collections.Generic;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("SetMedicationSchedulesInactive")]
    public class SetMedicationSchedulesInactive
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<ScheduleType> Schedules { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Bit, ParameterName = "inactive")]
        public bool Inactive { get; set; }
    }
}