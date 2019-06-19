using TransafeRx.Shared.Data;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Linq;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;

namespace TransafeRx.Services.NotificationService
{
    public class APNSMessages
    {
        public static void Main(string[] args)
        {
            using (var db = new TransafeRxEntities())
            {
                try
                {
                    var deviceTokenUsers = db.GetAllDeviceTokensWithUser().ToList();//Where(x => x.UserId == "3fd5b550-c517-4ff5-9825-c55b37d50175").ToList();
                    var allMedicationsNotTaken = db.GetAllMedicationsNotTakenWindow(61).ToList();//Where(x => x.UserId == "3fd5b550-c517-4ff5-9825-c55b37d50175").ToList();

                    //var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                    var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,

                    var apnsBroker = new ApnsServiceBroker(config);

                    apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                    {
                        aggregateEx.Handle(ex =>
                        {
                            if (ex is ApnsNotificationException)
                            {
                                var notificationException = (ApnsNotificationException)ex;
                                var apnsNotification = notificationException.Notification;
                                var statusCode = notificationException.ErrorStatusCode;

                                Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                            }
                            else
                            {
                                Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                            }

                            return true;
                        });
                    };

                    apnsBroker.OnNotificationSucceeded += (notification) =>
                    {
                        Console.WriteLine("Apple Notification Sent!");
                    };

                    apnsBroker.Start();

                    foreach(var medicationNotTaken in allMedicationsNotTaken)
                    {
                        GetAllDeviceTokensWithUser_Result user = deviceTokenUsers.Where(x => x.UserId == medicationNotTaken.UserId).FirstOrDefault();
                        if(user == null)
                        {
                            continue;
                        }
                        if (user.AdmissionTypeId.HasValue)
                        {
                            if (user.AdmissionTypeId.Value == 1)
                            {
                                db.AddMedicationActivityAdmitted(user.UserId, medicationNotTaken.ScheduleId, medicationNotTaken.UserMedicationId);
                                continue;
                            }
                        }

                        List<GetAllDeviceTokensWithUser_Result> users = deviceTokenUsers.Where(x => x.UserId == medicationNotTaken.UserId).ToList();
                        //GetAllDeviceTokensWithUser_Result deviceUser = deviceTokenUsers.Where(x => x.UserId == medicationNotTaken.UserId).Last();

                        foreach (var deviceUser in users)
                        {
                            if (!string.IsNullOrEmpty(deviceUser.Token))
                            {
                                string reminder = "";
                                int section = 0;
                                if (medicationNotTaken.ScheduleTime.Hours.Equals(DateTime.Now.Hour))
                                {
                                    section = medicationNotTaken.ScheduleTime.Hours;

                                    if (medicationNotTaken.ScheduleTime.Hours < 12)
                                    {
                                        reminder = String.Format("Time to take your {0} AM medication(s)!", medicationNotTaken.ScheduleTime.Hours);
                                    }
                                    else if (medicationNotTaken.ScheduleTime.Hours > 12 && medicationNotTaken.ScheduleTime.Hours < 24)
                                    {
                                        reminder = String.Format("Time to take your {0} PM medication(s)!", (medicationNotTaken.ScheduleTime.Hours - 12));
                                    }
                                    else if (medicationNotTaken.ScheduleTime.Hours == 12)
                                    {
                                        reminder = String.Format("Time to take your 12 PM medication(s)!");
                                    }
                                    else
                                    {
                                        reminder = String.Format("Time to take your {0} AM medication(s)!", (medicationNotTaken.ScheduleTime.Hours - 12));
                                    }

                                    try
                                    {
                                        apnsBroker.QueueNotification(new ApnsNotification
                                        {
                                            DeviceToken = deviceUser.Token,
                                            Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"anticipate.mp3\",\"alert\":{\"title\":\"" + "Medication Reminder" + "\",\"body\":\"" + reminder + "\"}},\"Section\":\"" + section + "\"}")
                                        });
                                        db.AddMessage(medicationNotTaken.ScheduleId, null, deviceUser.TokenId);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.StackTrace);
                                        db.AddMessage(medicationNotTaken.ScheduleId, e.StackTrace, deviceUser.TokenId);
                                    }
                                }
                            }
                        }
                    }

                    //foreach (var user in deviceTokenUsers)
                    //{
                    //    List<GetAllMedicationsNotTakenWindow_Result> medicationsNotTaken = allMedicationsNotTaken.Where(x => x.UserId == user.UserId).ToList();
                    //    if (user.AdmissionTypeId.HasValue)
                    //    {
                    //        if (user.AdmissionTypeId.Value == 1)
                    //        {
                    //            foreach (GetAllMedicationsNotTakenWindow_Result medication in medicationsNotTaken)
                    //            {
                    //                db.AddMedicationActivityAdmitted(user.UserId, medication.ScheduleId, medication.UserMedicationId);
                    //            }
                    //            break;
                    //        }
                    //    }

                    //    if (!string.IsNullOrEmpty(user.Token))
                    //    {
                    //        string reminder = "";
                    //        int section = 0;
                    //        if (medicationsNotTaken.Count > 0)
                    //        {
                    //            List<GetAllMedicationsNotTakenWindow_Result> MedicationsToTake = new List<GetAllMedicationsNotTakenWindow_Result>();
                    //            foreach (var medication in medicationsNotTaken)
                    //            {
                    //                if (medication.ScheduleTime.Hours.Equals(DateTime.Now.Hour))
                    //                {
                    //                    section = medication.ScheduleTime.Hours;

                    //                    if (medication.ScheduleTime.Hours < 12)
                    //                    {
                    //                        reminder = String.Format("Time to take your {0} AM medication(s)!", medication.ScheduleTime.Hours);
                    //                    }
                    //                    else if (medication.ScheduleTime.Hours > 12 && medication.ScheduleTime.Hours < 24)
                    //                    {
                    //                        reminder = String.Format("Time to take your {0} PM medication(s)!", (medication.ScheduleTime.Hours - 12));
                    //                    }
                    //                    else if(medication.ScheduleTime.Hours == 12)
                    //                    {
                    //                        reminder = String.Format("Time to take your 12 PM medication(s)!");
                    //                    }
                    //                    else
                    //                    {
                    //                        reminder = String.Format("Time to take your {0} AM medication(s)!", (medication.ScheduleTime.Hours - 12));
                    //                    }
                    //                    MedicationsToTake.Add(medication);
                    //                }
                    //            }

                    //            if (MedicationsToTake.Count() > 0)
                    //            {
                    //                try
                    //                {
                    //                    apnsBroker.QueueNotification(new ApnsNotification
                    //                    {
                    //                        DeviceToken = user.Token,
                    //                        Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"anticipate.mp3\",\"alert\":{\"title\":\"" + "Medication Reminder" + "\",\"body\":\"" + reminder + "\"}},\"Section\":\"" + section + "\"}")
                    //                    });
                    //                    foreach (GetAllMedicationsNotTakenWindow_Result medication in MedicationsToTake)
                    //                    {
                    //                        db.AddMessage(medication.ScheduleId, null, user.TokenId);
                    //                    }
                    //                }
                    //                catch (Exception e)
                    //                {
                    //                    Console.WriteLine(e.StackTrace);
                    //                    foreach (GetAllMedicationsNotTakenWindow_Result medication in MedicationsToTake)
                    //                    {
                    //                        db.AddMessage(medication.ScheduleId, e.StackTrace, user.TokenId);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    var fbs = new FeedbackService(config);
                    fbs.FeedbackReceived += (string deviceToken, DateTime timestamp) =>
                    {
                        //remove token from database
                        //timestamp is time token was expired
                        Console.WriteLine(String.Format("FeedbackReceived: {0} : {1}"), deviceToken, timestamp.ToString());
                        GetAllDeviceTokensWithUser_Result token = deviceTokenUsers.Where(x => x.Token == deviceToken).FirstOrDefault();
                        if (token != null)
                        {
                            db.UpdateDeviceTokenExpired(token.TokenId, true);
                        }
                    };
                    fbs.Check();

                    apnsBroker.Stop();
                }
                catch (Exception e)
                {
                    MailMessage message = new MailMessage(from, to);
                    message.Body = "An error has occured in TransafeRx Medication Notification Service.  Please see below for details.\r\n";
                    message.Body += "Stack Trace: " + e.StackTrace + "\r\n";

                    message.Subject = "TransafeRx Medication Notification Service Error Report";
                    client.Send(message);

                    message.Dispose();
                }
            }
        }
    }
}
