using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TransafeRx.Models;


namespace TransafeRx.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : ApplicationController
    {

        [HttpGet]
        public ActionResult Index()
        {
            var vm = new ReportViewModel();
            var users = db.GetUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                AspUserName = x.AspUserName,
                TacrolimusCV = x.TacrolimusCV
            }).ToList();
            users.Add(new UserViewModel("", "", "", "Select All"));
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            var aggregateList = new List<SelectListItem>()
            {
                new SelectListItem{ Value = "1", Text = "Day", Selected = true},
                new SelectListItem{ Value = "2", Text = "Week", Selected = false},
                new SelectListItem{ Value = "3", Text = "Month", Selected = false},
                new SelectListItem{ Value = "4", Text = "Year", Selected = false}
            };
            vm.AggregateSelectList = new SelectList(aggregateList, "Value", "Text");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;

            return View(vm);
        }

        [HttpGet]
        public ActionResult UserActions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveSummaryReport(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult BloodPressureChart(ReportViewModel reportViewModel, string userId, string StartDate, string EndDate, int? aggType)
        {
            var bpGroups = db.GetBloodPressureMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), aggType).ToList();

            reportViewModel.SBPSeries = bpGroups.Select(x => x.Systolic);
            reportViewModel.DBPSeries = bpGroups.Select(x => x.Diastolic);
            reportViewModel.PulseSeries = bpGroups.Select(x => x.Pulse);
            reportViewModel.DateSeries = bpGroups.Select(x => (x.ReadingDateUTC.Value.ToShortDateString()));

            return PartialView(reportViewModel);
        }

        public ActionResult BPMeasurements_Read([DataSourceRequest]DataSourceRequest request, ReportViewModel reportViewModel, string userId, string StartDate, string EndDate, int? aggType)
        {
            var bpGroups = db.GetBloodPressureMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), aggType).ToList();
            reportViewModel.BloodPressureTable.Columns.Add("Email", typeof(string)).Caption = "Email";
            reportViewModel.BloodPressureTable.Columns.Add("SBP", typeof(int)).Caption = "SBP";
            reportViewModel.BloodPressureTable.Columns.Add("DBP", typeof(int)).Caption = "DBP";
            reportViewModel.BloodPressureTable.Columns.Add("Pulse", typeof(int)).Caption = "Pulse";
            reportViewModel.BloodPressureTable.Columns.Add("ReadingDate", typeof(string)).Caption = "ReadingDate";

            foreach (var Group in bpGroups)
            {
                var row = reportViewModel.BloodPressureTable.NewRow();
                row[0] = Group.Email;
                row[1] = Group.Systolic;
                row[2] = Group.Diastolic;
                row[3] = Group.Pulse;
                switch (aggType)
                {
                    case 1://day
                        row[4] = Convert.ToDateTime(Group.ReadingDateUTC.ToString()).ToString("MM/dd/yyyy");
                        break;
                    case 2://week
                        row[4] = Convert.ToDateTime(Group.ReadingDateUTC.ToString()).ToString("MM/dd/yyyy");
                        break;
                    case 3://month
                        row[4] = Convert.ToDateTime(Group.ReadingDateUTC.ToString()).ToString("MMM yyyy");
                        break;
                    case 4://year
                        row[4] = Convert.ToDateTime(Group.ReadingDateUTC.ToString()).ToString("yyyy");
                        break;
                    default:
                        row[4] = Convert.ToDateTime(Group.ReadingDateUTC.ToString()).ToString("MM/dd/yyyy");
                        break;
                }

                reportViewModel.BloodPressureTable.Rows.Add(row);
            }
            return Json(reportViewModel.BloodPressureTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GlucoseChart(ReportViewModel reportViewModel, string userId, string StartDate, string EndDate, int? aggType)
        {
            var glucoseGroups = db.GetGlucoseMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), aggType).ToList();

            reportViewModel.GlucoseSeries = glucoseGroups.Select(x => x.Glucose);
            reportViewModel.DateSeries = glucoseGroups.Select(x => (x.ReadingDateUTC.ToShortDateString()));
            return PartialView(reportViewModel);
        }

        public ActionResult GlucoseMeasurements_Read([DataSourceRequest]DataSourceRequest request, ReportViewModel reportViewModel, string userId, string StartDate, string EndDate, int? aggType)
        {
            var glucoseGroups = db.GetGlucoseMeasurementsChart(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), aggType).ToList();
            reportViewModel.GlucoseTable.Columns.Add("Email", typeof(string)).Caption = "Email";
            reportViewModel.GlucoseTable.Columns.Add("Glucose", typeof(int)).Caption = "Glucose";
            reportViewModel.GlucoseTable.Columns.Add("ReadingDate", typeof(string)).Caption = "ReadingDate";

            foreach (var Group in glucoseGroups)
            {
                var row = reportViewModel.GlucoseTable.NewRow();
                row[0] = Group.Email;
                row[1] = Group.Glucose;
                switch (aggType)
                {
                    case 1://day
                        row[2] = Convert.ToDateTime(Group.ReadingDateUTC.ToLocalTime().ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        break;
                    case 2://week
                        row[2] = Convert.ToDateTime(Group.ReadingDateUTC.ToLocalTime().ToString()).ToString("MM/dd/yyyy");
                        break;
                    case 3://month
                        row[2] = Convert.ToDateTime(Group.ReadingDateUTC.ToLocalTime().ToString()).ToString("MMM yyyy");
                        break;
                    case 4://year
                        row[2] = Convert.ToDateTime(Group.ReadingDateUTC.ToLocalTime().ToString()).ToString("yyyy");
                        break;
                    default:
                        row[2] = Convert.ToDateTime(Group.ReadingDateUTC.ToLocalTime().ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        break;
                }

                reportViewModel.GlucoseTable.Rows.Add(row);
            }
            return Json(reportViewModel.GlucoseTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Adherence()
        {
            var vm = new AdherenceViewModel();
            var users = db.GetUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                AspUserName = x.AspUserName
            }).ToList();
            vm.UserSelectList = new SelectList(users, "UserId", "DisplayName");
            vm.StartDate = DateTime.Now;
            vm.EndDate = DateTime.Now;

            return View(vm);
        }

        public ActionResult AdherenceChart(AdherenceViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var adherenceScore = db.GetMedicationAdherenceOverAllScore(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).SingleOrDefault();

            if (adherenceScore.HasValue)
            {
                reportViewModel.AdherenceScore = (int)adherenceScore;
            }
            else
            {
                reportViewModel.AdherenceScore = 0;
            }

            return PartialView(reportViewModel);
        }

        public ActionResult MedAdherence_Read([DataSourceRequest]DataSourceRequest request, AdherenceViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var adherenceGroups = db.GetMedicationAdherenceByMed(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList();

            reportViewModel.MedAdherenceTable.Columns.Add("Medication", typeof(string)).Caption = "Medication";
            reportViewModel.MedAdherenceTable.Columns.Add("ScheduleDate", typeof(string)).Caption = "ScheduleDate";
            reportViewModel.MedAdherenceTable.Columns.Add("ActivityDate", typeof(string)).Caption = "ActivityDate";
            reportViewModel.MedAdherenceTable.Columns.Add("Status", typeof(string)).Caption = "Status";
            reportViewModel.MedAdherenceTable.Columns.Add("Score", typeof(double)).Caption = "Score";

            foreach (var Group in adherenceGroups)
            {
                var row = reportViewModel.MedAdherenceTable.NewRow();
                row[0] = Group.DrugName;
                row[1] = Convert.ToDateTime(Group.ScheduleDate.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                if (Group.ActivityDate != null)
                {
                    row[2] = Convert.ToDateTime(Group.ActivityDate.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                    if (Group.ActivityTypeId.Equals(1))
                    {
                        row[3] = "Taken";
                    }
                    else
                    {
                        row[3] = "Skipped";
                    }
                }
                else
                {
                    row[3] = "Missed";
                    row[2] = "";
                }
                row[4] = Group.Score;

                reportViewModel.MedAdherenceTable.Rows.Add(row);
            }
            return Json(reportViewModel.MedAdherenceTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

    }

}