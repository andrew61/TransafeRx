using TransafeRx.Shared.Data;
using TransafeRx.API.Models;
using System.Linq;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/BloodPressure")]
    public class BloodPressureController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetBloodPressureMeasurements(UserId).ToList();
            return Ok(measurements);
        }

        [Route("Chart")]
        [HttpPost]
        public IHttpActionResult Chart(ChartModel model)
        {
            var measurements = db.GetBloodPressureMeasurementsChart(UserId, model.StartDate, model.EndDate, model.Aggregate).ToList();
            return Ok(measurements);
        }

        [HttpPost]
        public IHttpActionResult Post(BloodPressureMeasurement measurement)
        {
            db.AddBloodPressureMeasurement(UserId, measurement.Systolic, measurement.Diastolic, measurement.Pulse, measurement.ReadingDate, measurement.Model);
            return Ok();
        }
    }
}