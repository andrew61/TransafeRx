using TransafeRx.Shared.Data;
using TransafeRx.API.Models;
using System.Linq;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/BloodGlucose")]
    public class BloodGlucoseController : ApplicationController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var measurements = db.GetBloodGlucoseMeasurements(UserId).ToList();
            return Ok(measurements);
        }

        [Route("Chart")]
        [HttpPost]
        public IHttpActionResult Chart(ChartModel model)
        {
            var measurements = db.GetGlucoseMeasurementsChart(UserId, model.StartDate, model.EndDate, model.Aggregate).ToList();
            return Ok(measurements);
        }

        [HttpPost]
        public IHttpActionResult Post(BloodGlucoseMeasurement measurement)
        {
            db.AddBloodGlucoseMeasurement(UserId, measurement.GlucoseLevel, measurement.ReadingDate, measurement.Model);
            return Ok();
        }
    }
}