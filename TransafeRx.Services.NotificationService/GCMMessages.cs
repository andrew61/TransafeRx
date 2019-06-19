namespace TransafeRx.Services.NotificationService
{
    public class GCMMessages
    {
        public static void Main(string[] args)
        {
            //using (var db = new TransafeRxEntities())
            //{
            //    var reminders = db.GetMedicationReminders().Where(x => x.TokenTypeId == 2).ToList();

            //    if (reminders.Any())
            //    {
            //        var gcmBroker = new GcmServiceBroker(config);

            //        gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            //        {

            //            aggregateEx.Handle(ex =>
            //            {

            //                if (ex is GcmNotificationException)
            //                {
            //                    var notificationException = (GcmNotificationException)ex;
            //                    var gcmNotification = notificationException.Notification;
            //                    var description = notificationException.Description;

            //                    //Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
            //                }
            //                else if (ex is GcmMulticastResultException)
            //                {
            //                    var multicastException = (GcmMulticastResultException)ex;

            //                    foreach (var succeededNotification in multicastException.Succeeded)
            //                    {
            //                        //Console.WriteLine($"GCM Notification Failed: ID={succeededNotification.MessageId}");
            //                    }

            //                    foreach (var failedKvp in multicastException.Failed)
            //                    {
            //                        var n = failedKvp.Key;
            //                        var e = failedKvp.Value;

            //                        //Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
            //                    }

            //                }
            //                else if (ex is DeviceSubscriptionExpiredException)
            //                {
            //                    var expiredException = (DeviceSubscriptionExpiredException)ex;

            //                    var oldId = expiredException.OldSubscriptionId;
            //                    var newId = expiredException.NewSubscriptionId;

            //                    //Console.WriteLine($"Device RegistrationId Expired: {oldId}");

            //                    if (!string.IsNullOrWhiteSpace(newId))
            //                    {
            //                        //Console.WriteLine($"Device RegistrationId Changed To: {newId}");
            //                    }
            //                }
            //                else if (ex is RetryAfterException)
            //                {
            //                    var retryException = (RetryAfterException)ex;
            //                    //Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
            //                }
            //                else
            //                {
            //                    //Console.WriteLine("GCM Notification Failed for some unknown reason");
            //                }

            //                return true;
            //            });
            //        };

            //        gcmBroker.OnNotificationSucceeded += (notification) =>
            //        {
            //            Console.WriteLine("GCM Notification Sent!");
            //        };

            //        gcmBroker.Start();

            //        foreach (var reminder in reminders)
            //        {
            //            gcmBroker.QueueNotification(new GcmNotification
            //            {
            //                RegistrationIds = new List<string> {
            //                        reminder.Token
            //                    },
            //                Notification = JObject.Parse("{ \"body\" : \"It's time to take your medication!\", \"title\" : \"TransafeRx\" }")
            //            });
            //        }

            //        gcmBroker.Stop();
            //    }
            //}
        }
    }
}
