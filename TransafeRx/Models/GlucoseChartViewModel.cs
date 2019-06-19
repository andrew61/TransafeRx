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
    public class GlucoseChartViewModel
    {
        public string UserId { get; set; }
        public SelectList AggregateSelectList { get; set; }
        public DataTable GlucoseTable { get; set; }
        public IEnumerable<int> GlucoseSeries { get; set; }
        public List<string> Colors { get; set; }
        public Random Random { get; set; }

        public GlucoseChartViewModel()
        {
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