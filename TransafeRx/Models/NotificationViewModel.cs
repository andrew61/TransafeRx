using System;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class NotificationViewModel
    {
        [UIHint("NullableForeignKey")]
        public int? NotificationTypeId { get; set; }
        public int NotificationPreferencesId { get; set; }
        public int? NotificationDays { get; set; }
        public string UserId { get; set; }
    }
}