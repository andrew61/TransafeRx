using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using System.Collections.Generic;
using System.Data;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("AddUpdateMedicationSchedules")]
    public class AddUpdateMedicationSchedules
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "schedules")]
        public List<ScheduleType> Schedules { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Int, Direction = ParameterDirection.Output)]
        public int GroupId { get; set; }
    }
}