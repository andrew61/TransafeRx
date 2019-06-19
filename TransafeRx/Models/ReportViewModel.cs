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
    public class ReportViewModel
    {
        public string UserId { get; set; }
        public string GlucoseId { get; set; }
        public SelectList UserSelectList { get; set; }
        public SelectList AggregateSelectList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DataTable BloodPressureTable { get; set; }
        public IEnumerable<int> SBPSeries { get; set; }
        public IEnumerable<int> DBPSeries { get; set; }
        public IEnumerable<int> PulseSeries { get; set; }
        public IEnumerable<String> DateSeries { get; set; }
        public DataTable GlucoseTable { get; set; }
        public IEnumerable<int> GlucoseSeries { get; set; }
        public Random Random { get; set; }
        public List<string> Colors { get; set; }
        
        public ReportViewModel()
        {
            BloodPressureTable = new DataTable();
            GlucoseTable = new DataTable();
            Random = new Random();
            Colors = new List<string>();
        }

        public string GetRandomColor()
        {
            string color = string.Format("#{0:X6}", Random.Next(0x1000000));

            while (Colors.Contains(color))
            {
                color = string.Format("#{0:X6}", Random.Next(0x1000000));
            }
            Colors.Add(color);

            return color;
        }
    }
}