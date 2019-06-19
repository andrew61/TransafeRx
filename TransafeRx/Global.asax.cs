using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TransafeRx
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["MailOnError"]))
            {
                Exception ex = Server.GetLastError().InnerException;

                MailMessage message = new MailMessage(from, to);
                message.Body = "An error has occured in TransafeRx.  Please see below for details.\r\n";

                if (HttpContext.Current != null)
                {
                    message.Body += "URL: " + HttpContext.Current.Request.Url.AbsoluteUri + "\r\n";
                    message.Body += "Page: " + HttpContext.Current.Request.Url.AbsolutePath + "\r\n";
                }
                if (ex != null)
                {
                    message.Body += "Message: " + ex.Message + "\r\n";
                    message.Body += "Source: " + ex.Source + "\r\n";
                    message.Body += "Stack Trace: " + ex.StackTrace + "\r\n";
                }
                else
                {
                    message.Body += Server.GetLastError().ToString();
                }
                message.Subject = "TransafeRx Error Report";
                client.Send(message);

                message.Dispose();
            }
        }
    }
}
