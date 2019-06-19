using TransafeRx.Shared.Data;
using TransafeRx.Shared.Models;
using NodaTime;
using System.Linq;

namespace TransafeRx.Shared.Utils
{
    public static class DateHelper
    {
        public static TimeZoneAdjustedDateInfo GetTimeZoneAdjustedDateInfo(string userId, Instant instant)
        {
            using (var db = new TransafeRxEntities())
            {
                var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(db.GetUserTimeZone(userId).SingleOrDefault() ?? "UTC");

                return new TimeZoneAdjustedDateInfo
                {
                    DateUTC = instant.ToDateTimeUtc(),
                    DateDTO = instant.InZone(timeZone).ToDateTimeOffset(),
                    DateCTZ = timeZone.Id
                };
            }
        }

        public static string GetUserTimeZone(string userId)
        {
            using (var db = new TransafeRxEntities())
            {
                var timeZone = db.GetUserTimeZone(userId).SingleOrDefault() ?? "UTC";

                return timeZone;
            }
        }
    }
}
