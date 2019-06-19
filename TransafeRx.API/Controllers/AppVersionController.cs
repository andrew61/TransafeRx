using System.Linq;
using System.Web.Http;
using TransafeRx.Shared.Data;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/AppVersion")]
    public class AppVersionController : ApiController
    {
        protected TransafeRxEntities db = new TransafeRxEntities();

        [Route("AppId/{appId}")]
        [HttpGet]
        public IHttpActionResult Get(int appId)
        {
            var appVersion = db.GetLatestAppVersionByAppId(appId).SingleOrDefault();
            if (appVersion == null)
            {
                return NotFound();
            }
            return Ok(appVersion);
        }
    }
}