using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransafeRx.Models
{
    public class EpicPatientViewModel
    {
        public string MRN { get; set; }
        public string RecipientName { get; set; }
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(RecipientName) || string.IsNullOrEmpty(RecipientName))
                {
                    return "";
                }
                return RecipientName.Substring(RecipientName.IndexOf(",")+1).Trim();
            }

        }
            
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(RecipientName) || string.IsNullOrEmpty(RecipientName))
                {
                    return "";
                }
                return RecipientName.Substring(0, RecipientName.IndexOf(",")).Trim();
            }
        }
        
    }
}