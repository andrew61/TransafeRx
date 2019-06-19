using TransafeRx.Shared.Data;
using System.Linq;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/UserMedication")]
    public class UserMedicationController : ApplicationController
    {
        //[HttpGet]
        //public IHttpActionResult Get(int id)
        //{
        //    var medication = db.GetUserMedication(id).SingleOrDefault();
        //    return Ok(medication);
        //}

        [HttpGet]
        public IHttpActionResult Get()
        {
            var medications = db.GetUserMedicationsAPIByUserId(UserId, false).ToList();
            return Ok(medications);
        }

        [HttpGet]
        [Route("NotTaken")]
        public IHttpActionResult NotTaken()
        {
            var medications = db.GetMedicationsNotTaken(UserId).ToList();
            return Ok(medications);
        }

        [HttpGet]
        [Route("NotTakenHour/{hour}")]
        public IHttpActionResult NotTakenHour(int hour)
        {
            var medications = db.GetMedicationsNotTakenByHour(UserId, hour).ToList();
            return Ok(medications);
        }

        //[HttpGet]
        //[Route("Medications/AsNeeded")]
        //public IHttpActionResult AsNeeded()
        //{
        //    var medications = db.GetUserMedicationsAsNeeded(UserId).ToList();
        //    return Ok(medications);
        //}

        //[HttpPost]
        //public IHttpActionResult Post(UserMedication medication)
        //{
        //    var id = db.AddUpdateUserMedication(medication.UserMedicationId, UserId, medication.StartDate, medication.Route, medication.Instructions, System.Convert.ToInt32(medication.Quantity), null, null, null, null, null, null).SingleOrDefault();
        //    medication.UserMedicationId = id.Value;
        //    return Ok(medication);
        //}

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult Delete(UserMedication medication)
        {
            db.DeleteUserMedication(medication.UserMedicationId);
            return Ok();
        }
    }
}