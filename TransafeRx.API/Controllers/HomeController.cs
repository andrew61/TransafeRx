using TransafeRx.API.Security;
using System;
using System.Web.Mvc;

namespace TransafeRx.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            string id = Guid.NewGuid().ToString();
            ViewData["id"] = id;
            ViewData["secret"] = Helper.GetHash(id);

            return View();
        }
    }
}
