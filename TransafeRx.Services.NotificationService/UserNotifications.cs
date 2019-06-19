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
    public class UserNotifications
    {
        public static void Main(string[] args)
        {
            using (var db = new TransafeRxEntities())
            {
                try
                {
                    var deviceTokenUsers = db.GetAllDeviceTokensWithUser().ToList();

                    //var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                    //Shared.Properties.Resources.TransafeRx_DEV_PUSH, Shared.Properties.Settings.Default.ApplePushPW);
                    var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
                    Shared.Properties.Resources.TransafeRx_PROD_PUSH, Shared.Properties.Settings.Default.ApplePushPW);

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

                            //Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                        }
                            else
                            {
                            //Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        }

                            return true;
                        });
                    };

                    apnsBroker.OnNotificationSucceeded += (notification) =>
                    {
                        Console.WriteLine("Apple Notification Sent!");
                    };

                    apnsBroker.Start();

                    foreach (var user in deviceTokenUsers)
                    {
                        var notifications = db.GetAllUserNotifications(user.UserId).SingleOrDefault();

                        if (!string.IsNullOrEmpty(user.Token))
                        {
                            //blood pressure notification
                            if (notifications != null)
                            {
                                if (notifications.BpNotificationTypeId != null)
                                {
                                    if (notifications.BpDaysSinceLastNotification.HasValue)
                                    {
                                        if (notifications.BpDaysSinceLastReading.HasValue)
                                        {
                                            if (notifications.BpDaysSinceLastReading > notifications.BpNotificationDays && notifications.BpDaysSinceLastNotification >= notifications.BpNotificationDays)
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Pressure Reminder\",\"body\":\"" + "Time to take your Blood Pressure!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 1, user.TokenId);
                                            }
                                        }else
                                        {
                                            apnsBroker.QueueNotification(new ApnsNotification
                                            {
                                                DeviceToken = user.Token,
                                                Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Pressure Reminder\",\"body\":\"" + "Time to take your Blood Pressure!" + "\"}}}")
                                            });
                                            db.AddNotificationMessage(user.UserId, 1, user.TokenId);
                                        }
                                    }
                                    else
                                    {
                                        if (notifications.BpNotificationDays.HasValue)
                                        {
                                            if (notifications.BpDaysSinceLastReading.HasValue)
                                            {
                                                if (notifications.BpDaysSinceLastReading > notifications.BpNotificationDays)
                                                {
                                                    apnsBroker.QueueNotification(new ApnsNotification
                                                    {
                                                        DeviceToken = user.Token,
                                                        Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Pressure Reminder\",\"body\":\"" + "Time to take your Blood Pressure!" + "\"}}}")
                                                    });
                                                    db.AddNotificationMessage(user.UserId, 1, user.TokenId);
                                                }
                                            }
                                            else
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Pressure Reminder\",\"body\":\"" + "Time to take your Blood Pressure!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 1, user.TokenId);
                                            }
                                        }
                                    }
                                }
                            }
                            //blood glucose notification
                            if (notifications != null)
                            {
                                if (notifications.BgNotificationTypeId != null)
                                {
                                    if (notifications.BgDaysSinceLastNotification.HasValue)
                                    {
                                        if (notifications.BgDaysSinceLastReading.HasValue)
                                        {
                                            if (notifications.BgDaysSinceLastReading > notifications.BgNotificationDays && notifications.BgDaysSinceLastNotification >= notifications.BgNotificationDays)
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Glucose Reminder\",\"body\":\"" + "Time to take your Blood Glucose!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 2, user.TokenId);
                                            }
                                        }else
                                        {
                                            apnsBroker.QueueNotification(new ApnsNotification
                                            {
                                                DeviceToken = user.Token,
                                                Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Glucose Reminder\",\"body\":\"" + "Time to take your Blood Glucose!" + "\"}}}")
                                            });
                                            db.AddNotificationMessage(user.UserId, 2, user.TokenId);
                                        }
                                    }
                                    else
                                    {
                                        if (notifications.BgNotificationDays.HasValue)
                                        {
                                            if (notifications.BgDaysSinceLastReading.HasValue)
                                            {
                                                if (notifications.BgDaysSinceLastReading > notifications.BgNotificationDays)
                                                {
                                                    apnsBroker.QueueNotification(new ApnsNotification
                                                    {
                                                        DeviceToken = user.Token,
                                                        Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Glucose Reminder\",\"body\":\"" + "Time to take your Blood Glucose!" + "\"}}}")
                                                    });
                                                    db.AddNotificationMessage(user.UserId, 2, user.TokenId);
                                                }
                                            }
                                            else
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Blood Glucose Reminder\",\"body\":\"" + "Time to take your Blood Glucose!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 2, user.TokenId);
                                            }
                                        }
                                    }
                                }
                            }
                            //survey notification
                            if (notifications != null)
                            {
                                if (notifications.SnNotificationTypeId != null)
                                {
                                    if (notifications.SnDaysSinceLastNotification.HasValue)
                                    {
                                        if (notifications.SnDaysSinceLastReading.HasValue)
                                        {
                                            if (notifications.SnDaysSinceLastReading > notifications.SnNotificationDays && notifications.SnDaysSinceLastNotification >= notifications.SnNotificationDays)
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Symptom Survey Reminder\",\"body\":\"" + "Time to take the Symptom Survey!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 3, user.TokenId);
                                            }
                                        }else
                                        {
                                            apnsBroker.QueueNotification(new ApnsNotification
                                            {
                                                DeviceToken = user.Token,
                                                Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Symptom Survey Reminder\",\"body\":\"" + "Time to take the Symptom Survey!" + "\"}}}")
                                            });
                                            db.AddNotificationMessage(user.UserId, 3, user.TokenId);
                                        }
                                    }
                                    else
                                    {
                                        if (notifications.SnNotificationDays.HasValue)
                                        {
                                            if (notifications.SnDaysSinceLastReading.HasValue)
                                            {
                                                if (notifications.SnDaysSinceLastReading > notifications.SnNotificationDays)
                                                {
                                                    apnsBroker.QueueNotification(new ApnsNotification
                                                    {
                                                        DeviceToken = user.Token,
                                                        Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Symptom Survey Reminder\",\"body\":\"" + "Time to take the Symptom Survey!" + "\"}}}")
                                                    });
                                                    db.AddNotificationMessage(user.UserId, 3, user.TokenId);
                                                }
                                            }
                                            else
                                            {
                                                apnsBroker.QueueNotification(new ApnsNotification
                                                {
                                                    DeviceToken = user.Token,
                                                    Payload = JObject.Parse("{\"aps\":{\"content-available\":\"1\",\"sound\":\"alarm\",\"alert\":{\"title\":\"Symptom Survey Reminder\",\"body\":\"" + "Time to take the Symptom Survey!" + "\"}}}")
                                                });
                                                db.AddNotificationMessage(user.UserId, 3, user.TokenId);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    apnsBroker.Stop();
                }
                catch (Exception e)
                {
                    MailMessage message = new MailMessage(from, to);
                    message.Body = "An error has occured in TransafeRx User Notification Service.  Please see below for details.\r\n";
                    message.Body += "Stack Trace: " + e.StackTrace + "\r\n";

                    message.Subject = "TransafeRx User Notification Service Error Report";
                    client.Send(message);

                    message.Dispose();
                }
            }
        }
    }
}
