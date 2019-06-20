using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Mail;
using System.Configuration;
using System.Web.Routing;
using TransafeRx.Shared.Data;

namespace TransafeRx.API.Controllers
{
    [RoutePrefix("api/Email")]
    public class EmailController: ApplicationController
    {
        [Route("Admission")]
        [HttpPost]
        public IHttpActionResult PostAdmission()
        {
            var user = db.GetUser(UserId).SingleOrDefault();
            string body = "Subject: " + user.MRN + "\nHas been admitted to hospital.";
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            MailAddress from = new MailAddress("tachl_support@musc.edu");
            MailMessage message = new MailMessage(from, to);
            message.To.Add("taberd@musc.edu");
            message.To.Add("fleminj@musc.edu");
            message.Body = body;
            message.Subject = "Subject Admission";
            client.Send(message);
            message.Dispose();

            db.AddAdmission(UserId, 1, DateTime.Now);
            return Ok();
        }

        [Route("Discharge")]
        [HttpPost]
        public IHttpActionResult PostDischarge()
        {
            var user = db.GetUser(UserId).SingleOrDefault();
            string body = "Subject: " + user.MRN + "\nHas been discharged from hospital.";
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            MailAddress from = new MailAddress("tachl_support@musc.edu");
            MailMessage message = new MailMessage(from, to);
            message.To.Add("taberd@musc.edu");
            message.To.Add("fleminj@musc.edu");
            message.Body = body;
            message.Subject = "Subject Discharge";
            client.Send(message);
            message.Dispose();

            db.AddAdmission(UserId, 2, DateTime.Now);
            return Ok();
        }

        [Route("Visit")]
        [HttpPost]
        public IHttpActionResult PostVisit()
        {
            var user = db.GetUser(UserId).SingleOrDefault();
            string body = "Subject: " + user.MRN + "\nHas visited the ER.";
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            MailAddress from = new MailAddress("tachl_support@musc.edu");
            MailMessage message = new MailMessage(from, to);
            message.To.Add("taberd@musc.edu");
            message.To.Add("fleminj@musc.edu");
            message.Body = body;
            message.Subject = "Subject ER Visit";
            client.Send(message);
            message.Dispose();

            return Ok();
        }

        [Route("Medication")]
        [HttpPost]
        public IHttpActionResult PostMedication()
        {
            var user = db.GetUser(UserId).SingleOrDefault();
            string body = "Subject: " + user.MRN + "\nHas a medication change";
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            MailAddress from = new MailAddress("tachl_support@musc.edu");
            MailMessage message = new MailMessage(from, to);
            message.To.Add("taberd@musc.edu");
            message.To.Add("fleminj@musc.edu");
            message.Body = body;
            message.Subject = "Subject Medication Change";
            client.Send(message);
            message.Dispose();

            return Ok();
        }
    }
}
