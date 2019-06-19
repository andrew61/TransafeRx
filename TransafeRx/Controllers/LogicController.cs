using System.Linq;
using System.Web.Mvc;
using TransafeRx.Models;

namespace TransafeRx.Controllers
{
    public class LogicController : ApplicationController
    {
        public ActionResult LogicBuilder()
        {
            return View("_LogicBuilder");
        }

        public ActionResult Condition(LogicBuilderViewModel vm)
        {
            return PartialView("_Condition", vm);
        }

        public ActionResult ConditionPlaceholder(LogicBuilderViewModel vm)
        {
            return PartialView("_ConditionPlaceholder", vm);
        }

        public JsonResult Items()
        {
            var items = db.GetItemTypes().Where(x => x.LogicBuilder).ToList();

            items.Add(new Shared.Data.GetItemTypes_Result { Name = "Role" });

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Quizzes()
        //{
        //    var quizzes = db.GetQuizzes().ToList();

        //    return Json(quizzes, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult QuizQuestions(int quizId)
        //{
        //    var questions = db.GetQuizQuestions(quizId).ToList();

        //    return Json(questions, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult QuizAnswers(int questionId)
        //{
        //    var answers = db.GetQuizAnswers(questionId).ToList().Select(x => new QuizAnswerViewModel()
        //    {
        //        AnswerId = x.AnswerId,
        //        QuestionId = x.QuestionId,
        //        AnswerText = x.AnswerText,
        //        AnswerOrder = x.AnswerOrder,
        //        Correct = x.Correct
        //    });

        //    return Json(answers, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult Surveys()
        {
            var surveys = db.GetSurveys().ToList();

            return Json(surveys, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyQuestions(int surveyId)
        {
            var questions = db.GetSurveyQuestions(surveyId).ToList();

            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyQuestionsList(int surveyId)
        {
            var questionTypes = db.GetSurveyQuestionTypes().ToList();
            var questions = db.GetSurveyQuestions(surveyId).Where(x => questionTypes.Any(y => x.QuestionTypeId == y.QuestionTypeId && y.HasOptions)).ToList();

            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyQuestionCategories(int questionId)
        {
            var categories = db.GetSurveyQuestionCategories(questionId).ToList().Select(x => new SurveyQuestionCategoryViewModel()
            {
                CategoryId = x.CategoryId,
                QuestionId = x.QuestionId,
                Name = x.Name,
                CategoryOrder = x.CategoryOrder
            });

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyQuestionOptions(int questionId, int? categoryId)
        {
            var options = db.GetSurveyQuestionOptions(questionId, categoryId).ToList().Select(x => new SurveyQuestionOptionViewModel()
            {
                OptionId = x.OptionId,
                QuestionId = x.QuestionId,
                OptionText = x.OptionText,
                OptionOrder = x.OptionOrder
            });

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Forms()
        //{
        //    var forms = db.GetForms().ToList();

        //    return Json(forms, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult FormFields(int formId)
        //{
        //    var fields = db.GetFormFields(formId).Where(x => x.LogicBuilder).ToList();

        //    return Json(fields, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult FormFieldOptions(int fieldId)
        //{
        //    var options = db.GetFormFieldOptions(fieldId).ToList();

        //    return Json(options, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Roles()
        //{
        //    var roles = RoleManager.Roles.Where(x => x.Name != "Admin").ToList();

        //    return Json(roles, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Agreements()
        //{
        //    var agreements = db.GetAgreements().ToList();

        //    return Json(agreements, JsonRequestBehavior.AllowGet);
        //}
    }
}