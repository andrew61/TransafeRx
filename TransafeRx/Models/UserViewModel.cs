using System.ComponentModel.DataAnnotations;
using System;

namespace TransafeRx.Models
{
    public class UserViewModel
    {
        private bool _isActive = true;
        public string UserId { get; set; }
        [UIHint("NullableForeignKey")]
        [Display(Name = "Role")]
        public bool? MedChanged { get; set; }
        public string RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MRN { get; set; }
        [Display(Name = "Active")]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }
        public int? TacrolimusCV { get; set; }
        public int? SBPAvg { get; set; }
        public int? DBPAvg { get; set; }
        public int? PulseAvg { get; set; }
        public int? GlucoseAvg { get; set; }
        public int? MissedAppts { get; set; }
        public DateTime? LastMissedAppt { get; set; }
        public bool? MedReviewed { get; set; }
        public int? MedAdh { get; set; }
        public int? MedRefillAdh { get; set; }
        public int? SurveyScore { get; set; }
        public bool? SurveyRisk { get; set; }
        public int? RiskLevel { get; set; }
        [Display(Name = "Disable Motivational Messages")]
        public bool DisableMotivationalMessages { get; set; }
        public bool? NeedDischarge { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string AspUserName { get; set; }
        [Display(Name = "Research #")]
        public int? ResearchNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string PasswordVerify { get; set; }

        [UIHint("PatientsDropdown")]
        public EpicPatientViewModel EpicPatient { get; set; }

       // [ScaffoldColumn(false)]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                {
                    return AspUserName;
                }

                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        public UserViewModel(string UserId, string FirstName, string LastName, string Email)
        {
            this.UserId = UserId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.AspUserName = AspUserName;
            
        }

        public UserViewModel()
        {
            
        }
    }
}