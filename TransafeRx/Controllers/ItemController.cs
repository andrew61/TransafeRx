using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
using TransafeRx.Models;
using TransafeRx.Shared.Models;

namespace TransafeRx.Controllers
{
    public class ItemController : ApplicationController
    {
        public ActionResult Surveys()
        {
            var vm = new SurveysViewModel();
            var templates = db.GetItemTemplates(3).ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.TemplateId.ToString()
            });

            vm.TemplateSelectList = new SelectList(templates, "Value", "Text");

            return View(vm);
        }

        public JsonResult SurveyQuestionTypes()
        {
            var questionTypes = db.GetSurveyQuestionTypes().OrderBy(x => x.Name).ToList();
            return Json(questionTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Surveys_Read([DataSourceRequest]DataSourceRequest request)
        {
            var surveys = db.GetSurveys().Where(x => !x.Deleted).ToList().Select(x => new SurveyViewModel
            {
                SurveyId = x.SurveyId,
                Name = x.Name,
                TemplateId = x.TemplateId,
                BodyBackgroundColor = x.BodyBackgroundColor,
                BodyColor = x.BodyColor,
                AllowRestart = x.AllowRestart
            });

            return Json(surveys.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Surveys_Create([DataSourceRequest]DataSourceRequest request, SurveyViewModel vm)
        {
            if (ModelState.IsValid)
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    vm.SurveyId = db.AddUpdateItem(vm.SurveyId, vm.ItemTypeId, vm.Name, vm.TemplateId, UserId).SingleOrDefault().GetValueOrDefault();
                    db.AddUpdateSurvey(vm.SurveyId, vm.AllowRestart);
                    tx.Commit();
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Surveys_Update([DataSourceRequest]DataSourceRequest request, SurveyViewModel vm)
        {
            if (ModelState.IsValid)
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    db.AddUpdateItem(vm.SurveyId, vm.ItemTypeId, vm.Name, vm.TemplateId, UserId).SingleOrDefault();
                    db.AddUpdateSurvey(vm.SurveyId, vm.AllowRestart);
                    tx.Commit();
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Surveys_Destroy([DataSourceRequest]DataSourceRequest request, SurveyViewModel vm)
        {
            db.DeleteSurvey(vm.SurveyId);

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult SurveyQuestions_Read([DataSourceRequest]DataSourceRequest request, int surveyId)
        {
            var questions = db.GetSurveyQuestions(surveyId).Where(x => !x.Deleted).ToList().Select(x => new SurveyQuestionViewModel
            {
                QuestionId = x.QuestionId,
                SurveyId = x.SurveyId,
                QuestionType = new SurveyQuestionTypeViewModel
                {
                    QuestionTypeId = x.QuestionTypeId,
                    Name = x.QuestionTypeName
                },
                QuestionTypeId = x.QuestionTypeId,
                Name = x.Name,
                QuestionText = x.QuestionText,
                QuestionImage = x.QuestionImage,
                QuestionOrder = x.QuestionOrder,
                OptionImage = x.OptionImage,
                Body = x.Body,
                Required = x.Required,
                Options = db.GetSurveyQuestionOptions(x.QuestionId, null).Select(y => new SurveyQuestionOptionViewModel
                {
                    OptionId = y.OptionId,
                    QuestionId = y.QuestionId,
                    CategoryId = y.CategoryId,
                    OptionText = y.OptionText,
                    OptionImage = y.OptionImage,
                    OptionValue = y.OptionValue,
                    OptionOrder = y.OptionOrder,
                    ShapeType = y.ShapeType.HasValue ? (ShapeType)y.ShapeType.Value : (ShapeType?)null,
                    Coordinates = y.Coordinates,
                    Feedback = y.Feedback
                }).ToList()
            });

            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SurveyQuestions_Create([DataSourceRequest]DataSourceRequest request, SurveyQuestionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.QuestionType.QuestionTypeId == 12)
                {
                    if (string.IsNullOrEmpty(vm.OptionImage))
                    {
                        ModelState.AddModelError("OptionImage", "Option Image is required");
                    }
                }

                if (vm.QuestionType.QuestionTypeId == 14)
                {
                    if (string.IsNullOrEmpty(vm.Body))
                    {
                        ModelState.AddModelError("Body", "Body is required");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(vm.QuestionText))
                    {
                        ModelState.AddModelError("QuestionText", "Question is required");
                    }
                }

                if (ModelState.IsValid)
                {
                    vm.QuestionId = db.AddUpdateSurveyQuestion(vm.QuestionId, vm.SurveyId, vm.QuestionType.QuestionTypeId, vm.Name, vm.QuestionText, vm.QuestionImage,
                        vm.OptionImage, vm.Body, vm.Required).SingleOrDefault().GetValueOrDefault();
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestions_Update([DataSourceRequest]DataSourceRequest request, SurveyQuestionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.QuestionType.QuestionTypeId == 12)
                {
                    if (string.IsNullOrEmpty(vm.OptionImage))
                    {
                        ModelState.AddModelError("OptionImage", "Option Image is required");
                    }
                }

                if (vm.QuestionType.QuestionTypeId == 14)
                {
                    if (string.IsNullOrEmpty(vm.Body))
                    {
                        ModelState.AddModelError("Body", "Body is required");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(vm.QuestionText))
                    {
                        ModelState.AddModelError("QuestionText", "Question is required");
                    }
                }

                if (ModelState.IsValid)
                {
                    db.AddUpdateSurveyQuestion(vm.QuestionId, vm.SurveyId, vm.QuestionType.QuestionTypeId, vm.Name, vm.QuestionText, vm.QuestionImage, vm.OptionImage, vm.Body, vm.Required);
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestions_Destroy([DataSourceRequest]DataSourceRequest request, SurveyQuestionViewModel vm)
        {
            db.DeleteSurveyQuestion(vm.QuestionId);

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestions_ChangeOrder(int questionId, int order)
        {
            db.ChangeSurveyQuestionOrder(questionId, order);
            return null;
        }

        public ActionResult SurveyQuestionOptions_Read([DataSourceRequest]DataSourceRequest request, int questionId, int? categoryId)
        {
            var options = db.GetSurveyQuestionOptions(questionId, categoryId).ToList().Select(x => new SurveyQuestionOptionViewModel
            {
                OptionId = x.OptionId,
                QuestionId = x.QuestionId,
                CategoryId = x.CategoryId,
                OptionText = x.OptionText,
                OptionImage = x.OptionImage,
                OptionValue = x.OptionValue,
                OptionOrder = x.OptionOrder,
                Feedback = x.Feedback
            });

            return Json(options.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SurveyQuestionOptions_Create([DataSourceRequest]DataSourceRequest request, SurveyQuestionOptionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.OptionId = db.AddUpdateSurveyQuestionOption(vm.OptionId, vm.QuestionId, vm.CategoryId, vm.OptionText, vm.OptionImage, vm.OptionValue, vm.ShapeTypeValue,
                    vm.Coordinates, vm.Feedback).SingleOrDefault().GetValueOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionOptions_CreateExisting(int questionId, int existingQuestionId)
        {
            db.GetSurveyQuestionOptions(existingQuestionId, null).ToList().ForEach(x =>
             {
                 db.AddUpdateSurveyQuestionOption(null, questionId, null, x.OptionText, x.OptionImage, x.OptionValue, null, null, null);
             });

            return null;
        }

        [HttpPost]
        public ActionResult SurveyQuestionOptions_Update([DataSourceRequest]DataSourceRequest request, SurveyQuestionOptionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateSurveyQuestionOption(vm.OptionId, vm.QuestionId, vm.CategoryId, vm.OptionText, vm.OptionImage, vm.OptionValue, vm.ShapeTypeValue,
                    vm.Coordinates, vm.Feedback);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionOptions_Destroy([DataSourceRequest]DataSourceRequest request, SurveyQuestionOptionViewModel vm)
        {
            db.DeleteSurveyQuestionOption(vm.OptionId);

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionOptions_ChangeOrder(int optionId, int order)
        {
            db.ChangeSurveyQuestionOptionOrder(optionId, order);
            return null;
        }

        public ActionResult SurveyQuestionCategories_Read([DataSourceRequest]DataSourceRequest request, int questionId)
        {
            var categories = db.GetSurveyQuestionCategories(questionId).ToList().Select(x => new SurveyQuestionCategoryViewModel
            {
                CategoryId = x.CategoryId,
                QuestionId = x.QuestionId,
                Name = x.Name,
                CategoryOrder = x.CategoryOrder
            });

            return Json(categories.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SurveyQuestionCategories_Create([DataSourceRequest]DataSourceRequest request, SurveyQuestionCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var categoryId = db.AddUpdateSurveyQuestionCategory(vm.CategoryId, vm.QuestionId, vm.Name).SingleOrDefault();
                vm.CategoryId = categoryId.GetValueOrDefault();
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionCategories_Update([DataSourceRequest]DataSourceRequest request, SurveyQuestionCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                db.AddUpdateSurveyQuestionCategory(vm.CategoryId, vm.QuestionId, vm.Name);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionCategories_ChangeOrder(int categoryId, int order)
        {
            db.ChangeSurveyQuestionCategoryOrder(categoryId, order);
            return null;
        }

        public JsonResult SurveyQuestions(int surveyId)
        {
            var questions = db.GetSurveyQuestions(surveyId).Where(x => x.HasOptions).ToList();

            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyQuestionCategories(int questionId)
        {
            var categories = db.GetSurveyQuestionCategories(questionId).ToList();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SurveyQuestionLogic_Read([DataSourceRequest]DataSourceRequest request, int questionId)
        {
            var logic = db.GetSurveyQuestionLogic(questionId).ToList().Select(x => new SurveyQuestionLogicViewModel
            {
                LogicId = x.LogicId,
                QuestionId = x.QuestionId,
                Expression = x.Expression,
                Components = x.Components,

                LogicAction = new SurveyQuestionLogicActionViewModel()
                {
                    ActionId = x.ActionId,
                    Name = x.ActionName
                },
                LogicActionQuestion = new SurveyLogicActionQuestionViewModel()
                {
                    QuestionId = x.ActionQuestionId.GetValueOrDefault(),
                    Name = x.ActionQuestionName
                }
            });

            return Json(logic.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SurveyQuestionLogic_Create([DataSourceRequest]DataSourceRequest request, SurveyQuestionLogicViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.LogicAction.ActionId != 2)
                {
                    vm.LogicActionQuestion.QuestionId = (int?)null;
                    vm.LogicActionQuestion.Name = null;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.EvaluateLogic(UserId, vm.Expression);
                        vm.LogicId = db.AddUpdateSurveyQuestionLogic(
                            vm.LogicId, vm.QuestionId, vm.LogicAction.ActionId, vm.LogicActionQuestion.QuestionId, vm.Expression, vm.Components).SingleOrDefault().GetValueOrDefault();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Logic", "An unexpected error has occurred.");
                    }
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionLogic_Update([DataSourceRequest]DataSourceRequest request, SurveyQuestionLogicViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.LogicAction.ActionId != 2)
                {
                    vm.LogicActionQuestion.QuestionId = (int?)null;
                    vm.LogicActionQuestion.Name = null;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.EvaluateLogic(UserId, vm.Expression);
                        db.AddUpdateSurveyQuestionLogic(
                            vm.LogicId, vm.QuestionId, vm.LogicAction.ActionId, vm.LogicActionQuestion.QuestionId, vm.Expression, vm.Components).SingleOrDefault().GetValueOrDefault();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Logic", "An unexpected error has occurred.");
                    }
                }
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SurveyQuestionLogic_Destroy([DataSourceRequest]DataSourceRequest request, SurveyQuestionLogicViewModel vm)
        {
            db.DeleteSurveyQuestionLogic(vm.LogicId);

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult SurveyQuestionLogicActions()
        {
            var actions = db.GetSurveyQuestionLogicActions().ToList();

            return Json(actions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyLogicActionQuestions(int surveyId, int questionId)
        {
            var questions = db.GetSurveyQuestions(surveyId).ToList();
            var question = questions.Single(x => x.QuestionId == questionId);

            questions = questions.Where(x => x.QuestionOrder > question.QuestionOrder).OrderBy(x => x.QuestionOrder).ToList();

            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SurveyQuestionImageMap_Preview(int id, int os = 1)
        {
            var model = db.GetSurveyQuestion(id).SingleOrDefault();

            ViewData["os"] = os;
            ViewBag.Options = db.GetSurveyQuestionOptions(id, null).ToList();

            return View(model);
        }

        public ActionResult SurveyDesign(int surveyId)
        {
            var vm = new SurveyViewModel();
            var survey = db.GetSurvey(surveyId).Single();

            vm.SurveyId = survey.SurveyId;
            vm.ListColor = survey.ListColor;
            vm.ListBackgroundColor = survey.ListBackgroundColor;
            vm.ListBackgroundAlpha = survey.ListBackgroundAlpha ?? 1;
            vm.ListFontId = survey.ListFontId;
            vm.ListFontSize = survey.ListFontSize;
            vm.SelectedColor = survey.SelectedColor;
            vm.SelectedBackgroundColor = survey.SelectedBackgroundColor;
            vm.SelectedBackgroundAlpha = survey.SelectedBackgroundAlpha ?? 1;
            vm.SelectedFontId = survey.SelectedFontId;
            vm.SelectedFontSize = survey.SelectedFontSize;
            vm.SelectedIconType = survey.SelectedIconType;
            vm.SelectedIconColor = survey.SelectedIconColor;
            vm.ButtonColor = survey.ButtonColor;
            vm.ButtonBackgroundColor = survey.ButtonBackgroundColor;
            vm.ButtonBackgroundAlpha = survey.ButtonBackgroundAlpha ?? 1;
            vm.ButtonFontId = survey.ButtonFontId;
            vm.ButtonFontSize = survey.ButtonFontSize;
            vm.ProgressBarColor = survey.ProgressBarColor;
            vm.ProgressBarBackgroundColor = survey.ProgressBarBackgroundColor;
            vm.ProgressBarBackgroundAlpha = survey.ProgressBarBackgroundAlpha ?? 1;
            vm.TooltipColor = survey.TooltipColor;
            vm.TooltipBackgroundColor = survey.TooltipBackgroundColor;
            vm.TooltipBackgroundAlpha = survey.TooltipBackgroundAlpha ?? 1;
            vm.TooltipFontId = survey.TooltipFontId;
            vm.TooltipFontSize = survey.TooltipFontSize;

            vm.HeaderTitle = survey.HeaderTitle;
            vm.HeaderImage = survey.HeaderImage;
            vm.HeaderFontId = survey.HeaderFontId;
            vm.HeaderFontSize = survey.HeaderFontSize;
            vm.HeaderColor = survey.HeaderColor;
            vm.HeaderBackgroundColor = survey.HeaderBackgroundColor;
            vm.HeaderBackgroundImage = survey.HeaderBackgroundImage;
            vm.HeaderEffect = survey.HeaderEffect;
            vm.BackButtonType = survey.BackButtonType;
            vm.BackButtonColor = survey.BackButtonColor;
            vm.BackButtonBackgroundColor = survey.BackButtonBackgroundColor;
            vm.BodyFontId = survey.BodyFontId;
            vm.BodyFontSize = survey.BodyFontSize;
            vm.BodyColor = survey.BodyColor;
            vm.BodyBackgroundColor = survey.BodyBackgroundColor;
            vm.BodyBackgroundPortraitImage = survey.BodyBackgroundPortraitImage;

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult SurveyDesign(SurveyViewModel vm)
        {
            using (var tx = db.Database.BeginTransaction())
            {
                db.UpdateItemDesign(vm.SurveyId, vm.HeaderTitle, vm.HeaderImage, vm.HeaderFontId, vm.HeaderFontSize, vm.HeaderColor, vm.HeaderBackgroundColor,
                                    vm.HeaderBackgroundImage, vm.HeaderEffect, vm.BackButtonType, vm.BackButtonColor, vm.BackButtonBackgroundColor, vm.BodyFontId,
                                    vm.BodyFontSize, vm.BodyColor, vm.BodyBackgroundColor, vm.BodyBackgroundPortraitImage);

                db.UpdateSurveyDesign(vm.SurveyId, vm.ListColor, vm.ListBackgroundColor, vm.ListBackgroundAlpha, vm.ListFontId, vm.ListFontSize, vm.SelectedColor, vm.SelectedBackgroundColor, vm.SelectedBackgroundAlpha, vm.SelectedFontId,
                                    vm.SelectedFontSize, vm.SelectedIconType, vm.SelectedIconColor, vm.ButtonColor, vm.ButtonBackgroundColor, vm.ButtonBackgroundAlpha, vm.ButtonFontId, vm.ButtonFontSize,
                                    vm.ProgressBarColor, vm.ProgressBarBackgroundColor, vm.ProgressBarBackgroundAlpha, vm.TooltipColor, vm.TooltipBackgroundColor, vm.TooltipBackgroundAlpha, vm.TooltipFontId, vm.TooltipFontSize);
                tx.Commit();
            }

            return Json(vm);
        }
    }
}