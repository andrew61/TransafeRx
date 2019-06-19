using TransafeRx.Shared.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using Twilio;

namespace MessagingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using (var db = new TransafeRxEntities())
                {
                    db.Database.CommandTimeout = 0;
                    var messages = db.GetMessagesToSend_30().ToList();//db.GetMedAdhMsgs().ToList();

                    if (messages.Any())
                    {
                        var twilioClient = new TwilioRestClient(ConfigurationManager.AppSettings["twilio_sid"], ConfigurationManager.AppSettings["twilio_authToken"]);

                        foreach (var message in messages)
                        {
                            try
                            {
                                Twilio.Message tMessage = twilioClient.SendMessage(ConfigurationManager.AppSettings["twilioNbr"], message.MobPhone, message.MsgText);

                                db.UpdateMsgSendQueue(message.mqID);
                                //db.InsertPatientMessageSent(message.UserId, message.MsgText, tMessage.DateCreated, message.MsgID, tMessage.Sid, tMessage.Status, tMessage.ErrorMessage);
                                //db.AddPersonalizedSurveyMessageHistory(message.UserId, message.MsgID);
                            }
                            catch (Exception e)
                            {
                                using (var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["mail_host"]))
                                {
                                    string im = (e.InnerException != null) ? e.InnerException.Message : "";
                                    string body = e.Message + ';' + im;
                                    var mMsg = new MailMessage("tachl_support@musc.edu", ConfigurationManager.AppSettings["SupportEmail"]);
                                    mMsg.Subject = "TransafeRx Personalized Msgs In Loop Error Report";
                                    mMsg.Body = body;
                                    mMsg.IsBodyHtml = true;
                                    smtpClient.Send(mMsg);
                                }
                                //throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["mail_host"]))
                {
                    string im = (ex.InnerException != null) ? ex.InnerException.Message : "";
                    string body = ex.Message + ';' + im;
                    var mMsg = new MailMessage("tachl_support@musc.edu", ConfigurationManager.AppSettings["SupportEmail"]);
                    mMsg.Subject = "TransafeRx Personalized Msgs Report";
                    mMsg.Body = body;
                    mMsg.IsBodyHtml = true;
                    smtpClient.Send(mMsg);
                }
                throw;
            }
        }
    }
}
