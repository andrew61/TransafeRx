using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TransafeRx.Shared.Data;
using System.Web;
using System.Web.Mvc;

namespace TransafeRx.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        protected TransafeRxEntities db = new TransafeRxEntities();
        protected ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
    }
}