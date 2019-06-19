namespace TransafeRx.Models
{
    public class SurveyViewModel : ItemViewModel
    {
        public int SurveyId { get; set; }
        public override int ItemTypeId
        {
            get { return 3; }
        }

        public bool AllowRestart { get; set; }

        public string ListColor { get; set; }
        public string ListBackgroundColor { get; set; }
        public decimal ListBackgroundAlpha { get; set; }
        public int? ListFontId { get; set; }
        public int? ListFontSize { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedBackgroundColor { get; set; }
        public decimal SelectedBackgroundAlpha { get; set; }
        public int? SelectedFontId { get; set; }
        public int? SelectedFontSize { get; set; }
        public int? SelectedIconType { get; set; }
        public string SelectedIconColor { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonBackgroundColor { get; set; }
        public decimal ButtonBackgroundAlpha { get; set; }
        public int? ButtonFontId { get; set; }
        public int? ButtonFontSize { get; set; }
        public string ProgressBarColor { get; set; }
        public string ProgressBarBackgroundColor { get; set; }
        public decimal ProgressBarBackgroundAlpha { get; set; }
        public string TooltipColor { get; set; }
        public string TooltipBackgroundColor { get; set; }
        public decimal TooltipBackgroundAlpha { get; set; }
        public int? TooltipFontId { get; set; }
        public int? TooltipFontSize { get; set; }
    }
}