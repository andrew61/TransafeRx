using System.ComponentModel.DataAnnotations;

namespace TransafeRx.Models
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public virtual int ItemTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Template is required")]
        public int TemplateId { get; set; }

        public string ItemTypeName { get; set; }

        public string HeaderTitle { get; set; }
        public string HeaderImage { get; set; }
        public int? HeaderFontId { get; set; }
        public int? HeaderFontSize { get; set; }
        public string HeaderColor { get; set; }
        public string HeaderBackgroundColor { get; set; }
        public string HeaderBackgroundImage { get; set; }
        public decimal HeaderBackgroundImageAlpha { get; set; }
        public int? HeaderEffect { get; set; }
        public int? BackButtonType { get; set; }
        public string BackButtonColor { get; set; }
        public string BackButtonBackgroundColor { get; set; }
        public int? BodyFontId { get; set; }
        public int? BodyFontSize { get; set; }
        public string BodyColor { get; set; }
        public string BodyBackgroundColor { get; set; }
        public string BodyBackgroundPortraitImage { get; set; }
        public string BodyBackgroundLandscapeImage { get; set; }
        public decimal BodyBackgroundImageAlpha { get; set; }
    }
}