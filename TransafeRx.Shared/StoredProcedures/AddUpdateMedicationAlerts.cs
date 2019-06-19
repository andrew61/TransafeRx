using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using System.Collections.Generic;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("AddUpdateMedicationAlerts")]
    public class AddUpdateMedicationAlerts
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "alerts")]
        public List<MedicationAlertType> Alerts { get; set; }
    }
}