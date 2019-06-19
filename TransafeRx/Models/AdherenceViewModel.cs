using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class AdherenceViewModel
    {
        public string UserId { get; set; }
        public SelectList UserSelectList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DataTable MedAdherenceTable { get; set; }
        public DataTable RefillAdherenceTable { get; set; }
        public DataTable MedTable { get; set; }
        public int AdherenceScore { get; set; }
        public int RefillAdherenceScore { get; set; }
        
        public AdherenceViewModel()
        {
            MedAdherenceTable = new DataTable();
            RefillAdherenceTable = new DataTable();
        }
    }
}