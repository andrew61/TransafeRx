using EntityFrameworkExtras.EF6;
using System;

namespace TransafeRx.Shared.Models
{
    [UserDefinedTableType("SurveyAnswerType")]
    public class SurveyAnswer
    {
        [UserDefinedTableTypeColumn(1)]
        public int? CategoryId { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public int? OptionId { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public string AnswerText { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public DateTimeOffset AnswerDateDTO { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public DateTime AnswerDateUTC { get; set; }

        [UserDefinedTableTypeColumn(6)]
        public string AnswerDateCTZ { get; set; }

        [UserDefinedTableTypeColumn(7)]
        public int? Rank { get; set; }
    }
}