using TransafeRx.Models;
using TransafeRx.Shared.Models;
using EntityFrameworkExtras.EF6;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Mvc;
using TransafeRx.Shared.Data;
using TransafeRx.Shared.StoredProcedures;
//using NodaTime;

using Twilio;

namespace TransafeRx.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class UsersController : ApplicationController
    {
        string masterUserId = "";
        [HttpGet]
        public ActionResult Index()
        {
            var userRoles = db.GetUserRoles().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();

            var notificationTypes = db.GetNotificationTypes().Select(x => new SelectListItem
            {
                Text = x.NotificationTypeName.ToString(),
                Value = x.NotificationTypeId.ToString()
            }).ToList();

            ViewData["NotificationTypes"] = notificationTypes;
            ViewData["UserRoles"] = userRoles;

            return View();
        }

        //public JsonResult NotificationTypes()
        //{
        //    var notificationTypes = db.GetNotificationTypes().ToList().Select(x => new NotificationTypeViewModel()
        //    {
        //        NotificationTypeId = x.NotificationTypeId,
        //        NotificationName = x.NotificationTypeName
        //    });

        //    return Json(notificationTypes, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = db.GetUsers().Select(x => new UserViewModel
            {
                UserId = x.UserId,
                RoleId = x.RoleId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                AspUserName = x.AspUserName,
                MRN = x.MRN,
                TacrolimusCV = x.TacrolimusCV,
                SBPAvg = x.SBPAvg,
                DBPAvg = x.DBPAvg,
                PulseAvg = x.PulseAvg,
                GlucoseAvg = x.GlucoseAvg,
                MissedAppts = x.MissedAppts,
                MedAdh = x.MedAdh,
                MedRefillAdh = x.MedRefillAdh,
                RiskLevel = x.RiskLevel,
                SurveyRisk = x.SurveyRisk,
                SurveyScore = x.SurveyScore,
                MedChanged = x.MedChanged,
                LastMissedAppt = x.LastMissedAppt,
                IsActive = x.IsActive,
                MedReviewed = x.MedReviewed,
                NeedDischarge = x.NeedDischarge,
                ResearchNumber = x.ResearchNumber,
                DisableMotivationalMessages = x.DisableMotivationalMessages
            }).ToList();

            return Json(users.ToDataSourceResult(request));
        }

        public ActionResult DischargePatient(string UserId)
        {
            if (UserId == null) return null;
            var person = db.AddAdmission(UserId, 2, DateTime.Now);
            return Json(person, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ChangePassword(string UserId, string Password)
        {
            if (UserId == null) return null;
            string token = await UserManager.GeneratePasswordResetTokenAsync(UserId);
            var resetResult = await UserManager.ResetPasswordAsync(UserId, token, Password);
            if (!resetResult.Succeeded)
            {
                //transaction.Rollback();
                //return GetErrorResult(resetResult);
            }
            return Json(resetResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendPatientSMS(string UserId, string message)
        {
            if (UserId == null) return null;

            var user = db.GetUser(UserId).SingleOrDefault();

            var tClient = new TwilioRestClient(ConfigurationManager.AppSettings["twilio_sid"], ConfigurationManager.AppSettings["twilio_authToken"]);
            tClient.SendMessage(ConfigurationManager.AppSettings["twilioNbr"], user.PhoneNumber, message);
            
            string sentBy = User.Identity.GetUserId();
            var sms = db.AddPatientSMS(UserId, message, sentBy, user.PhoneNumber);

            return Json(sms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notifications_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var notifications = db.GetNotificationPreferences(userId).ToList().Select(x => new NotificationViewModel()
            {
                NotificationTypeId = x.NotificationTypeId,
                UserId = x.UserId,
                NotificationDays = x.NotificationDays
            });

            return Json(notifications.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Users_Create([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(vm.Password))
                {
                    ModelState.AddModelError("User", "The Password field is required.");
                }

                if (string.IsNullOrEmpty(vm.PasswordVerify))
                {
                    ModelState.AddModelError("User", "You must verify your password.");
                }

                if (vm.Password != vm.PasswordVerify)
                {
                    ModelState.AddModelError("User", "The passwords do not match.");
                }

                if (ModelState.IsValid)
                {
                    using (var ts = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new ApplicationUser() { UserName = vm.AspUserName, Email = vm.AspUserName };
                            IdentityResult result = UserManager.Create(user, vm.Password);

                            if (result.Succeeded)
                            {
                                db.AddUpdateUser(user.Id, vm.RoleId, vm.FirstName, vm.LastName, vm.PhoneNumber, null, null, null, null, vm.MRN, vm.AspUserName, vm.IsActive, vm.ResearchNumber, vm.DisableMotivationalMessages).SingleOrDefault(); ts.Commit();

                                vm.UserId = user.Id;

                                db.AddUpdateUserMedicationFromEPIC(vm.MRN);
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("User", error);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ts.Rollback();
                            ModelState.AddModelError("Patient", "An unexpected error has occurred.");
                        }
                    }
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Users_Update([DataSourceRequest]DataSourceRequest request, UserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateUser(vm.UserId, vm.RoleId, vm.FirstName, vm.LastName, vm.PhoneNumber, null, null, null, null, vm.MRN, vm.AspUserName, vm.IsActive, vm.ResearchNumber, vm.DisableMotivationalMessages).SingleOrDefault();
            }
            
            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult Notifications_Create([DataSourceRequest]DataSourceRequest request, NotificationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var notificationPreferencesId = db.AddUpdateNotificationPreference(vm.NotificationPreferencesId, vm.UserId, vm.NotificationTypeId, vm.NotificationDays).SingleOrDefault();
                vm.NotificationPreferencesId = notificationPreferencesId.Value;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Notifications_Update([DataSourceRequest]DataSourceRequest request, NotificationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var notificationPreferencesId = db.AddUpdateNotificationPreference(vm.NotificationPreferencesId, vm.UserId, vm.NotificationTypeId, vm.NotificationDays).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserMedicationsImported_Read([DataSourceRequest]DataSourceRequest request, string mrn)
        {
            var userMedications = db.GetExternalDrugData(mrn).Select(x => new SurescriptsViewModel
            {
               // AlphaId = x.AlphaId,
                DrugName = x.DrugName,
                Instructions = x.Instructions,
                DispenseDate = Convert.ToDateTime(x.DispenseDate),
                MRN = x.MRN,
                DaysSupply = x.DaysSuppy,
                DispenseAmount = x.DispenseAmount
            }).ToList();

            return Json(userMedications.ToDataSourceResult(request));
        }

        public ActionResult PrednisoneImported_Read([DataSourceRequest]DataSourceRequest request, string mrn)
        {
            var userMedications = db.GetPrednisoneRefillData(mrn).Select(x => new SurescriptsViewModel
            {
                // AlphaId = x.AlphaId,
                DrugName = x.DrugName,
                Instructions = x.Instructions,
                DispenseDate = Convert.ToDateTime(x.DispenseDate),
                MRN = x.MRN,
                DaysSupply = x.DaysSuppy,
                DispenseAmount = x.DispenseAmount
            }).ToList();

            return Json(userMedications.ToDataSourceResult(request));
        }

        public ActionResult MedAdherence_Read([DataSourceRequest]DataSourceRequest request, AdherenceViewModel reportViewModel, string userId, string StartDate, string EndDate)
        {
            var adherenceGroups = db.GetMedicationAdherenceByMed(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList();

            reportViewModel.MedTable.Columns.Add("Medication", typeof(string)).Caption = "Medication";
            reportViewModel.MedTable.Columns.Add("ScheduleDate", typeof(string)).Caption = "ScheduleDate";
            reportViewModel.MedTable.Columns.Add("ActivityDate", typeof(string)).Caption = "ActivityDate";
            reportViewModel.MedTable.Columns.Add("Status", typeof(string)).Caption = "Status";
            reportViewModel.MedTable.Columns.Add("Score", typeof(double)).Caption = "Score";

            foreach (var Group in adherenceGroups)
            {
                var row = reportViewModel.MedTable.NewRow();
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

                reportViewModel.MedTable.Rows.Add(row);
            }
            return Json(reportViewModel.MedTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MedAdherenceSummary_Read([DataSourceRequest]DataSourceRequest request, MedAdherenceViewModel reportViewModel, string userId)
        {
            DateTime EndDate = DateTime.Now;
            DateTime StartDate = EndDate.AddDays(-14);
            
            var adherenceGroups = db.GetMedicationAdherenceByMed(userId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).Select(x => new MedAdherenceViewModel
            {
                Medication = x.DrugName,
                ScheduleDate = x.ScheduleDate,
                ActivityDate = x.ActivityDate,
                Status = x.ActivityTypeId == 1 ? "Taken" : "Skipped",
                Score = x.Score
            }).ToList();
            
            return Json(adherenceGroups.ToDataSourceResult(request));
        }

        public ActionResult _BPChart(BPChartViewModel reportViewModel, string userId)
        {
            var bpGroups = db.GetBloodPressureMeasurementsChart(userId, DateTime.Now.AddMonths(-1), DateTime.Now, null).ToList();

            reportViewModel.SBPSeries = bpGroups.Select(x => x.Systolic);
            reportViewModel.DBPSeries = bpGroups.Select(x => x.Diastolic);
            reportViewModel.PulseSeries = bpGroups.Select(x => x.Pulse);

            return PartialView(reportViewModel);
        }

        public ActionResult _TabStripBP(BPChartViewModel reportViewModel, string userId)
        {
            var bpGroups = db.GetBloodPressureMeasurementsChart(userId, DateTime.Now.AddMonths(-1), DateTime.Now, null).ToList();

            reportViewModel.SBPSeries = bpGroups.Select(x => x.Systolic);
            reportViewModel.DBPSeries = bpGroups.Select(x => x.Diastolic);
            reportViewModel.PulseSeries = bpGroups.Select(x => x.Pulse);

            return View(reportViewModel);
        }

        [HttpPost]
        public ActionResult BP_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            return Json(db.GetBloodPressureMeasurementsChart(userId, DateTime.Now.AddMonths(-2), DateTime.Now, 1));
        }

        public ActionResult BPData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var bpReadings = db.GetBloodPressureMeasurements(userId).Select(x => new BloodPressureViewModel
            {
                BloodPressureId = x.BloodPressureId,
                UserId = x.UserId,
                Systolic = x.Systolic,
                Diastolic = x.Diastolic,
                ReadingDateUTC = x.ReadingDate,
                ReadingDate = x.ReadingDate.ToLocalTime(),
                Pulse = x.Pulse
            }).ToList();

            return Json(bpReadings.ToDataSourceResult(request));
        }

        public ActionResult BP_Update([DataSourceRequest]DataSourceRequest request, BloodPressureViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string createdBy = User.Identity.GetUserId();
                db.EditBloodPressureMeasurement(vm.BloodPressureId, vm.Systolic, vm.Diastolic, vm.Pulse, vm.ReadingDate, createdBy);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult BP_Create([DataSourceRequest]DataSourceRequest request, BloodPressureViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string createdBy = User.Identity.GetUserId();
                db.AddBloodPressureMeasurementWithModified(vm.UserId, vm.Systolic, vm.Diastolic, vm.Pulse, vm.ReadingDate, createdBy);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Glucose_Update([DataSourceRequest]DataSourceRequest request, GlucoseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string createdBy = User.Identity.GetUserId();
                db.EditGlucoseMeasurement(vm.GlucoseId, vm.GlucoseLevel, vm.ReadingDate, createdBy);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Glucose_Create([DataSourceRequest]DataSourceRequest request, GlucoseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string createdBy = User.Identity.GetUserId();
                db.AddBloodGlucoseMeasurementModified(vm.UserId, vm.GlucoseLevel, vm.ReadingDate, createdBy);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult EpicPatients()
        {
            var epicPatients = db.GetEpicPatients().ToList().Select(x => new EpicPatientViewModel
            {
                MRN = x.MRN,
                RecipientName = x.RECIPIENT_NAME
            });

            return Json(epicPatients, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GlucoseChart(GlucoseChartViewModel reportViewModel, string userId)
        {
            var glucoseGroups = db.GetGlucoseMeasurementsChart(userId, DateTime.Now.AddMonths(-1), DateTime.Now, null).ToList();

            reportViewModel.GlucoseSeries = glucoseGroups.Select(x => x.Glucose);
            
            return PartialView(reportViewModel);
        }

        [HttpPost]
        public ActionResult Glucose_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var glucoseReadings = db.GetBloodGlucoseMeasurementsMonth(userId).Select(x => new GlucoseViewModel
            {
                GlucoseId = x.BloodGlucoseId,
                UserId = x.UserId,
                GlucoseLevel = x.GlucoseLevel,
                ReadingDateUTC = x.ReadingDate,
                ReadingDate = x.ReadingDate.ToLocalTime()
            }).ToList();

            return Json(glucoseReadings);
            //return Json(db.GetBloodGlucoseMeasurementsMonth(userId));
        }

        public ActionResult GlucoseData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var glucoseReadings = db.GetBloodGlucoseMeasurements(userId).Select(x => new GlucoseViewModel
            {
                GlucoseId = x.BloodGlucoseId,
                UserId = x.UserId,
                GlucoseLevel = x.GlucoseLevel,
                ReadingDateUTC = x.ReadingDate,
                ReadingDate = x.ReadingDate.ToLocalTime()
            }).ToList();

            return Json(glucoseReadings.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult UserMedications_Create([DataSourceRequest]DataSourceRequest request, UserMedicationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //var userMedicationId = db.AddUpdateUserMedication(vm.UserMedicationId, vm.UserId, vm.StartDate, vm.Route, vm.Instructions, 
                //    vm.DaysSupply, null, null, null, null, null, null).SingleOrDefault();
                //vm.UserMedicationId = userMedicationId.Value;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserMedications_Update([DataSourceRequest]DataSourceRequest request, UserMedicationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //db.AddUpdateUserMedication(vm.UserMedicationId, vm.UserId, vm.StartDate, vm.Route, vm.Instructions,
                //    vm.DaysSupply, null, null, null, null, null, null).SingleOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserMedications_Destroy([DataSourceRequest]DataSourceRequest request, UserMedicationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.DeleteUserMedication(vm.UserMedicationId);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Glucose_Destroy([DataSourceRequest]DataSourceRequest request, GlucoseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.DeleteGlucoseReading(vm.GlucoseId);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult BP_Destroy([DataSourceRequest]DataSourceRequest request, BloodPressureViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.DeleteBPReading(vm.BloodPressureId);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserMedications_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var reviewed = db.UpdateDateMedsReviewed(userId);

            var userMedications = db.GetUserMedications(userId, false).ToList().Select(x => new UserMedicationViewModel()
            {
                UserMedicationId = x.UserMedicationId,
                UserId = x.UserId,
                DrugName = x.DrugName,
                StartDate = x.StartDate,
                Instructions = x.Instructions,
                Route = x.Route,
                ModifiedDate = x.ModifiedDate
            }).ToList();

            return Json(userMedications.ToDataSourceResult(request));
        }

        public ActionResult ApptData_Read([DataSourceRequest]DataSourceRequest request, string mrn)
        {
            var appts = db.GetAppointments(mrn).ToList().Select(x => new AppointmentViewModel()
            {
                MRN = x.MRN,
                Contact_Date = x.CONTACT_DATE.Value,
                Enc_Type_Name = x.ENC_TYPE_NAME,
                Appt_Status_Name = x.APPT_STATUS_NAME
            }).ToList();

            return Json(appts.ToDataSourceResult(request));
        }

        public ActionResult PatientSMS_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var sms = db.GetPatientSMSHistory(userId).ToList().Select(x => new PatientSMSMessageViewModel()
            {
                SMSMessageId = x.SMSMessageId,
                Message = x.Message,
                UserId = x.UserId,
                SentBy = x.SentBy,
                DateSent = x.DateSent,
                PhoneNumberUsed = x.PhoneNumberUsed,
                SentByName = x.SentByName
            }).ToList();

            return Json(sms.ToDataSourceResult(request));
        }

        public ActionResult SymptomData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var appts = db.GetSurveyResults(userId).ToList().Select(x => new SymptomViewModel()
            {
                QuestionId = x.QuestionId,
                QuestionText = x.QuestionText,
                AnswerText = x.AnswerText,
                OptionValue = x.OptionValue.Value,
                AnswerDateUTC = x.AnswerDateUTC
            }).ToList();

            return Json(appts.ToDataSourceResult(request));
        }

        public ActionResult SymptomDataBySession_Read([DataSourceRequest]DataSourceRequest request, int sessionId)
        {
            var appts = db.GetSurveyResultsBySessionId(sessionId).ToList().Select(x => new SymptomViewModel()
            {
                QuestionId = x.QuestionId,
                QuestionText = x.QuestionText,
                AnswerText = x.AnswerText,
                OptionValue = x.OptionValue.Value,
                AnswerDateUTC = x.AnswerDateUTC
            }).ToList();

            return Json(appts.ToDataSourceResult(request));
        }

        public ActionResult SymptomHistoryData_Read([DataSourceRequest]DataSourceRequest request, string userId)
        {
            var appts = db.GetSurveyScoresForUser(userId).ToList().Select(x => new SymptomHistoryViewModel()
            {
                SessionId = x.sessionId,
                Score = x.Score,
                SurveyDate = x.SurveyDate
            }).ToList();

            return Json(appts.ToDataSourceResult(request));
        }
        public ActionResult TacrolimusData_Read([DataSourceRequest]DataSourceRequest request, string mrn)
        {
            var appts = db.GetTacrolimusResults(mrn).ToList().Select(x => new TacrolimusViewModel()
            {
                PATIENTEXTERNALID = x.PATIENTEXTERNALID,
                ORDERCLASSDESC = x.ORDERCLASSDESC,
                RESULTNUMERIC = x.RESULTNUMERIC,
                SPECIMENTAKENTIME = x.SPECIMENTAKENTIME.Value
            }).ToList();

            return Json(appts.ToDataSourceResult(request));
        }
        public JsonResult UserMedications(string userId)
        {
            var userMedications = db.GetUserMedications(userId, false).Select(x => new UserMedicationViewModel()
            {
                UserMedicationId = x.UserMedicationId,
                UserId = x.UserId,
                StartDate = x.StartDate,
                Route = x.Route,
                Instructions = x.Instructions,
                Quantity = Convert.ToInt32(x.Quantity)
            });

            return Json(userMedications, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserRoles()
        {
            var roles = db.GetUserRoles().Select(x => new AspNetRole()
            {
                Id = x.Id,
                Name = x.Name
            });

            return Json(roles, JsonRequestBehavior.AllowGet);
        }       
        
        public ActionResult UserSchedules_Read([DataSourceRequest]DataSourceRequest request, int? userMedicationId)
        {
            var schedules = db.GetSchedulesByUserMedicationId(userMedicationId, false).ToList().GroupBy(x => x.GroupId).Select(x => new ScheduleGroupViewModel()
            {
                UserId = x.First().UserId,
                UserMedicationId = userMedicationId.Value,
                DrugName = x.First().DrugName,
                StartDate = x.First().StartDate,
                EndDate = x.First().EndDate,
                ScheduleTime = DateTime.Today + x.First().ScheduleTime,
                Active = !x.First().Inactive,
                GroupId = x.Key.GetValueOrDefault(),
                Sunday = x.Any(y => y.DayOfWeek == 1),
                Monday = x.Any(y => y.DayOfWeek == 2),
                Tuesday = x.Any(y => y.DayOfWeek == 3),
                Wednesday = x.Any(y => y.DayOfWeek == 4),
                Thursday = x.Any(y => y.DayOfWeek == 5),
                Friday = x.Any(y => y.DayOfWeek == 6),
                Saturday = x.Any(y => y.DayOfWeek == 7)
            }).OrderBy(x => x.UserMedicationId);

            var userMedication = db.GetUserMedicationById(userMedicationId).Select(x => new UserMedicationViewModel
            {
                UserMedicationId = x.UserMedicationId,
                UserId = x.UserId,
                DrugName = x.DrugName,
                Route = x.Route,
                Instructions = x.Instructions,
                StartDate = x.StartDate
            }).ToList();

            //if (userMedication.Count > 0)
            //    Session["UserMedicationName"] = userMedication.First().Medication.DisplayName;

            return Json(schedules.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult UserSchedules_Create([DataSourceRequest]DataSourceRequest request, ScheduleGroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedules = db.GetSchedulesByUserMedicationId(vm.UserMedicationId, true).Where(x => x.GroupId == vm.GroupId).Select(x => new ScheduleType()
                {
                    ScheduleId = x.ScheduleId,
                    UserId = x.UserId,
                    UserMedicationId = x.UserMedicationId.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleTime = x.ScheduleTime,
                    GroupId = x.GroupId
                }).ToList();

                foreach (var schedule in schedules)
                {
                    schedule.StartDate = vm.StartDate;
                    schedule.EndDate = vm.EndDate;
                    schedule.ScheduleTime = vm.ScheduleTime.Value.TimeOfDay;
                    switch ((int)schedule.DayOfWeek)
                    {
                        case 1:
                            schedule.DayOfWeek = vm.Sunday ? schedule.DayOfWeek : 0;
                            break;
                        case 2:
                            schedule.DayOfWeek = vm.Monday ? schedule.DayOfWeek : 0;
                            break;
                        case 3:
                            schedule.DayOfWeek = vm.Tuesday ? schedule.DayOfWeek : 0;
                            break;
                        case 4:
                            schedule.DayOfWeek = vm.Wednesday ? schedule.DayOfWeek : 0;
                            break;
                        case 5:
                            schedule.DayOfWeek = vm.Thursday ? schedule.DayOfWeek : 0;
                            break;
                        case 6:
                            schedule.DayOfWeek = vm.Friday ? schedule.DayOfWeek : 0;
                            break;
                        case 7:
                            schedule.DayOfWeek = vm.Saturday ? schedule.DayOfWeek : 0;
                            break;
                    }
                }

                if (vm.Sunday && !schedules.Any(x => x.DayOfWeek == 1))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 1,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Monday && !schedules.Any(x => x.DayOfWeek == 2))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 2,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Tuesday && !schedules.Any(x => x.DayOfWeek == 3))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 3,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Wednesday && !schedules.Any(x => x.DayOfWeek == 4))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 4,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Thursday && !schedules.Any(x => x.DayOfWeek == 5))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 5,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Friday && !schedules.Any(x => x.DayOfWeek == 6))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 6,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Saturday && !schedules.Any(x => x.DayOfWeek == 7))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 7,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                var sp = new AddUpdateMedicationSchedules() { Schedules = schedules };
                db.Database.ExecuteStoredProcedure(sp);

                vm.GroupId = sp.GroupId;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserSchedules_Update([DataSourceRequest]DataSourceRequest request, ScheduleGroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedules = db.GetMedicationSchedules(vm.UserMedicationId).Where(x => x.GroupId == vm.GroupId).Select(x => new ScheduleType()
                {
                    ScheduleId = x.ScheduleId,
                    UserId = x.UserId,
                    UserMedicationId = x.UserMedicationId.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleTime = x.ScheduleTime,
                    GroupId = x.GroupId
                }).ToList();

                foreach (var schedule in schedules)
                {
                    schedule.StartDate = vm.StartDate;
                    schedule.EndDate = vm.EndDate;
                    schedule.ScheduleTime = vm.ScheduleTime.Value.TimeOfDay;
                    schedule.UserMedicationId = vm.UserMedicationId;
                    switch ((int)schedule.DayOfWeek)
                    {
                        case 1:
                            schedule.DayOfWeek = vm.Sunday ? schedule.DayOfWeek : 0;
                            break;
                        case 2:
                            schedule.DayOfWeek = vm.Monday ? schedule.DayOfWeek : 0;
                            break;
                        case 3:
                            schedule.DayOfWeek = vm.Tuesday ? schedule.DayOfWeek : 0;
                            break;
                        case 4:
                            schedule.DayOfWeek = vm.Wednesday ? schedule.DayOfWeek : 0;
                            break;
                        case 5:
                            schedule.DayOfWeek = vm.Thursday ? schedule.DayOfWeek : 0;
                            break;
                        case 6:
                            schedule.DayOfWeek = vm.Friday ? schedule.DayOfWeek : 0;
                            break;
                        case 7:
                            schedule.DayOfWeek = vm.Saturday ? schedule.DayOfWeek : 0;
                            break;
                    }
                }

                if (vm.Sunday && !schedules.Any(x => x.DayOfWeek == 1))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 1,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Monday && !schedules.Any(x => x.DayOfWeek == 2))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 2,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Tuesday && !schedules.Any(x => x.DayOfWeek == 3))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 3,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Wednesday && !schedules.Any(x => x.DayOfWeek == 4))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 4,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Thursday && !schedules.Any(x => x.DayOfWeek == 5))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 5,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Friday && !schedules.Any(x => x.DayOfWeek == 6))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 6,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                if (vm.Saturday && !schedules.Any(x => x.DayOfWeek == 7))
                {
                    schedules.Add(new ScheduleType()
                    {
                        UserId = vm.UserId,
                        UserMedicationId = vm.UserMedicationId,
                        StartDate = vm.StartDate,
                        EndDate = vm.EndDate,
                        DayOfWeek = 7,
                        ScheduleTime = vm.ScheduleTime.Value.TimeOfDay,
                        CreatedDateUTC = null,
                        CreatedDateDTO = null,
                        CreatedDateCTZ = null,
                        UpdatedDateUTC = null,
                        UpdatedDateDTO = null,
                        UpdatedDateCTZ = null
                    });
                }

                var sp = new AddUpdateMedicationSchedules() { Schedules = schedules };
                db.Database.ExecuteStoredProcedure(sp);

                var sp2 = new SetMedicationSchedulesInactive() { Schedules = schedules, Inactive = !vm.Active };
                db.Database.ExecuteStoredProcedure(sp2);

                vm.GroupId = sp.GroupId;
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult UserSchedules_Destroy([DataSourceRequest]DataSourceRequest request, ScheduleGroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedules = db.GetMedicationSchedules(vm.UserMedicationId).Where(x => x.GroupId == vm.GroupId).Select(x => new ScheduleType()
                {
                    ScheduleId = x.ScheduleId,
                    UserId = x.UserId,
                    UserMedicationId = x.UserMedicationId.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    ScheduleTime = x.ScheduleTime,
                    GroupId = x.GroupId
                }).ToList();

                var sp = new DeleteMedicationSchedules() { Schedules = schedules };
                db.Database.ExecuteStoredProcedure(sp);
            }
            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UserSchedulesExpired_Read([DataSourceRequest]DataSourceRequest request, int? userMedicationId)
        {
            var schedules = db.GetSchedulesByUserMedicationId(userMedicationId, true).ToList().GroupBy(x => x.GroupId).Select(x => new ScheduleGroupViewModel()
            {
                UserId = x.First().UserId,
                UserMedicationId = x.First().UserMedicationId.Value,
                DrugName = x.First().DrugName,
                StartDate = x.First().StartDate,
                EndDate = x.First().EndDate,
                ScheduleTime = DateTime.Today + x.First().ScheduleTime,
                Active = !x.First().Inactive,
                GroupId = x.Key.GetValueOrDefault(),
                Sunday = x.Any(y => y.DayOfWeek == 1),
                Monday = x.Any(y => y.DayOfWeek == 2),
                Tuesday = x.Any(y => y.DayOfWeek == 3),
                Wednesday = x.Any(y => y.DayOfWeek == 4),
                Thursday = x.Any(y => y.DayOfWeek == 5),
                Friday = x.Any(y => y.DayOfWeek == 6),
                Saturday = x.Any(y => y.DayOfWeek == 7)
            }).OrderBy(x => x.UserMedicationId);

            return Json(schedules.ToDataSourceResult(request));
        }
    }
}