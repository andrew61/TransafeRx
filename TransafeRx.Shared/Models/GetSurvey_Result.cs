using System.ComponentModel.DataAnnotations.Schema;

namespace TransafeRx.Shared.Data
{
    [NotMapped]
    public partial class GetSurvey_Result
    {
        public GetCurrentSurveySession_Result Session { get; set; }
    }
}