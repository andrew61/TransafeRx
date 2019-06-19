using TransafeRx.Shared.Models;
using EntityFrameworkExtras.EF6;
using System.Collections.Generic;

namespace TransafeRx.Shared.StoredProcedures
{
    [StoredProcedure("AddSurveyAnswers")]
    public class AddSurveyAnswers
    {
        [StoredProcedureParameter(System.Data.SqlDbType.Int, ParameterName = "surveyId")]
        public int SurveyId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.NVarChar, ParameterName = "userId")]
        public string UserId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Int, ParameterName = "sessionId")]
        public int SessionId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Int, ParameterName = "questionId")]
        public int QuestionId { get; set; }

        [StoredProcedureParameter(System.Data.SqlDbType.Udt, ParameterName = "answers")]
        public List<SurveyAnswer> Answers { get; set; }
    }
}