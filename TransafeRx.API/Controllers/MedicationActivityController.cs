using EntityFrameworkExtras.EF6;
using TransafeRx.Shared.Models;
using TransafeRx.Shared.StoredProcedures;
using System.Collections.Generic;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/MedicationActivity")]
    public class MedicationActivityController : ApplicationController
    {
        [HttpPost]
        public IHttpActionResult Post(List<MedicationActivityType> activity)
        {
            var sp = new AddMedicationActivityBatch() { UserId = UserId, Activity = activity };
            db.Database.ExecuteStoredProcedure(sp);
            return Ok();
        }
    }
}