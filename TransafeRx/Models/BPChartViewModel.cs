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
    public class BPChartViewModel
    {
        public string UserId { get; set; }
        public SelectList AggregateSelectList { get; set; }
        public DataTable BloodPressureTable { get; set; }
        public IEnumerable<int> SBPSeries { get; set; }
        public IEnumerable<int> DBPSeries { get; set; }
        public IEnumerable<int> PulseSeries { get; set; }
        public List<string> Colors { get; set; }
        public Random Random { get; set; }

        public BPChartViewModel()
        {
            BloodPressureTable = new DataTable();
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