using TransafeRx.Shared.Data;
using System.Web.Http;

namespace TransafeRx.API.Controllers
{
    public class LoginInformationController : ApplicationController
    {
        [HttpPost]
        public IHttpActionResult Post(LoginInformation info)
        {
            db.AddLoginInformation(UserId, info.DateDTO, info.Longitude, info.Latitude, info.Model, info.OS, info.Network, info.PhoneType, info.AppVersion, info.DateCTZ);
            return Ok();
        }
    }
}