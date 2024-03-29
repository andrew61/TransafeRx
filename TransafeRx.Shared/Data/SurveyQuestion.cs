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
    
    public partial class SurveyQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SurveyQuestion()
        {
            this.SurveyAnswers = new HashSet<SurveyAnswer>();
            this.SurveyQuestionCategories = new HashSet<SurveyQuestionCategory>();
            this.SurveyQuestionOptions = new HashSet<SurveyQuestionOption>();
        }
    
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string Name { get; set; }
        public string QuestionText { get; set; }
        public string QuestionImage { get; set; }
        public int QuestionOrder { get; set; }
        public string OptionImage { get; set; }
        public string Body { get; set; }
        public string Feedback { get; set; }
        public bool Required { get; set; }
        public bool Deleted { get; set; }
        public string ListColor { get; set; }
        public string ListBackgroundColor { get; set; }
        public Nullable<decimal> ListBackgroundAlpha { get; set; }
        public Nullable<int> ListFontId { get; set; }
        public Nullable<int> ListFontSize { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedBackgroundColor { get; set; }
        public Nullable<decimal> SelectedBackgroundAlpha { get; set; }
        public Nullable<int> SelectedFontId { get; set; }
        public Nullable<int> SelectedFontSize { get; set; }
        public Nullable<int> SelectedIconType { get; set; }
        public string SelectedIconColor { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonBackgroundColor { get; set; }
        public Nullable<decimal> ButtonBackgroundAlpha { get; set; }
        public Nullable<int> ButtonFontId { get; set; }
        public Nullable<int> ButtonFontSize { get; set; }
        public string ProgressBarColor { get; set; }
        public string ProgressBarBackgroundColor { get; set; }
        public Nullable<decimal> ProgressBarBackgroundAlpha { get; set; }
        public string TooltipColor { get; set; }
        public string TooltipBackgroundColor { get; set; }
        public Nullable<decimal> TooltipBackgroundAlpha { get; set; }
        public Nullable<int> TooltipFontId { get; set; }
        public Nullable<int> TooltipFontSize { get; set; }
        public string FeedbackColor { get; set; }
        public string FeedbackBackgroundColor { get; set; }
        public Nullable<decimal> FeedbackBackgroundAlpha { get; set; }
        public Nullable<int> FeedbackFontId { get; set; }
        public Nullable<int> FeedbackFontSize { get; set; }
        public string SeparatorColor { get; set; }
    
        public virtual Font Font { get; set; }
        public virtual Font Font1 { get; set; }
        public virtual Font Font2 { get; set; }
        public virtual Font Font3 { get; set; }
        public virtual Font Font4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestionCategory> SurveyQuestionCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestionOption> SurveyQuestionOptions { get; set; }
        public virtual SurveyQuestionType SurveyQuestionType { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
