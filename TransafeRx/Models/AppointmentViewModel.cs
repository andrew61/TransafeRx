using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class AppointmentViewModel
    {
        public string MRN { get; set; }
        public DateTime Contact_Date { get; set; }
        public string Enc_Type_Name { get; set; }
        public string Appt_Status_Name { get; set; }
    }
}