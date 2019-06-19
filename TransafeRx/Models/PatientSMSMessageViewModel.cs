using System;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class PatientSMSMessageViewModel
    {
        public int SMSMessageId { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public string SentBy { get; set; }
        public string SentByName { get; set; }
        public string PhoneNumberUsed { get; set; }
    }
    
}