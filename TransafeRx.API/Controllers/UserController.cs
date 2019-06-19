using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TransafeRx.Shared.Models;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApplicationController
    {
        [HttpGet]
        [Route("Admission")]
        public IHttpActionResult GetAdmission()
        {
            var admissionType = db.GetUserAdmission(UserId).SingleOrDefault();
            return Ok(admissionType);
        }

        [HttpGet]
        [Route("PersonalizedSurveyTaken")]
        public IHttpActionResult GetPersonalizedSurveyTaken()
        {
            var taken = db.GetUserPersonalizedSurveyTaken(UserId).SingleOrDefault();
            return Ok(taken);
        }

        [HttpGet]
        [Route("PhoneNumbers")]
        public IHttpActionResult GetPhoneNumbers()
        {
            var phoneNumbers = db.GetPhoneNumber(UserId).SingleOrDefault();
            return Ok(phoneNumbers);
        }

        [HttpGet]
        [Route("Admin")]
        public IHttpActionResult GetIsAdmin()
        {
            var isAdmin = db.GetIsAdmin(UserId).SingleOrDefault();
            return Ok(isAdmin.Value);
        }

        [HttpPost]
        [Route("APNSToken")]
        public IHttpActionResult PostAPNSToken(DeviceToken token)
        {
            db.AddDeviceToken(UserId, 1, token.Token);
            return Ok();
        }

        [HttpPost]
        [Route("GCMToken")]
        public IHttpActionResult PostGCMToken(DeviceToken token)
        {
            db.AddDeviceToken(UserId, 2, token.Token);
            return Ok();
        }
    }
}