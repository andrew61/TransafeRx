using Microsoft.AspNet.Identity;
using TransafeRx.Shared.Data;
using TransafeRx.Shared.Utils;
using TransafeRx.Shared.Models;
using System.Web.Http;
using NodaTime;

namespace TransafeRx.API.Controllers
{
    [Authorize]
    public class ApplicationController : ApiController
    {
        protected TransafeRxEntities db = new TransafeRxEntities();

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }

        public string UserName
        {
            get { return User.Identity.GetUserName(); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected TimeZoneAdjustedDateInfo GetTimeZoneAdjustedDateInfo(Instant instant)
        {
            return DateHelper.GetTimeZoneAdjustedDateInfo(UserId, instant);
        }
    }
}