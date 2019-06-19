//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransafeRx.Shared.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatientMessageSent
    {
        public int PatMessID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> MessageID { get; set; }
        public Nullable<System.DateTime> SentTime { get; set; }
        public string MessageText { get; set; }
        public string TwilioMessageSID { get; set; }
        public string TwilioMsgError { get; set; }
        public string TwilioMessageStatus { get; set; }
        public Nullable<int> ScheduleId { get; set; }
    
        public virtual Message Message { get; set; }
        public virtual User User { get; set; }
    }
}
