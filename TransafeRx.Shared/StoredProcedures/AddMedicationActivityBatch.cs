using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using System.Collections.Generic;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("AddMedicationActivity")]
    public class AddMedicationActivityBatch
    {
        [StoredProcedureParameter(System.Data.SqlDbType.NVarChar, ParameterName = "userId")]
        public string UserId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "activity")]
        public List<MedicationActivityType> Activity { get; set; }
    }
}