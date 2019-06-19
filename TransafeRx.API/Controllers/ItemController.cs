using EntityFrameworkExtras.EF6;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using TransafeRx.Shared.Data;
using TransafeRx.Shared.Models;
using TransafeRx.Shared.StoredProcedures;
using TransafeRx.Shared.Utils;
using System.Web.Http.Cors;
using NodaTime;
using Newtonsoft.Json;

namespace TransafeRx.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Item")]
    public class ItemController : ApplicationController
    {
        [Route("{itemId}")]
        public IHttpActionResult GetItem(int itemId, GetItemNavigation_Result navigation)
        {
            var item = db.GetItem(itemId).SingleOrDefault();
            var toolbarItems = db.GetItemToolbarItems(itemId).ToList();
            var logic = db.GetItemLogic(itemId).ToList();
            var visibleToolbarItems = new List<GetItemToolbarItems_Result>();
            var actions = new List<GetItemLogic_Result>();

            foreach (var toolbarItem in toolbarItems)
            {
                if (string.IsNullOrEmpty(toolbarItem.LogicExpression) || (db.EvaluateLogic(UserId, toolbarItem.LogicExpression).SingleOrDefault() ?? false))
                {
                    visibleToolbarItems.Add(toolbarItem);

                    var itemNavigation = db.GetToolbarItemNavigation(toolbarItem.ToolbarItemId).OrderBy(x => x.Priority).ToList();

                    foreach (var line in itemNavigation)
                    {
                        if (string.IsNullOrEmpty(line.LogicExpression) || (db.EvaluateLogic(UserId, line.LogicExpression).SingleOrDefault() ?? false))
                        {
                            toolbarItem.TargetItemId = line.TargetItemId;
                            toolbarItem.SelectedMenuItemId = line.SelectedMenuItemId;
                            toolbarItem.NavigationTypeId = line.NavigationTypeId;
                            toolbarItem.TransitionTypeId = line.TransitionTypeId;
                            break;
                        }
                    }
                }
            }

            foreach (var line in logic.Where(x => x.EventId == 1))
            {
                if (string.IsNullOrEmpty(line.Expression) || (db.EvaluateLogic(UserId, line.Expression).SingleOrDefault() ?? false))
                {
                    actions.Add(line);
                }
            }

            switch (item.ItemTypeId)
            {
                //case 1:
                //    var quiz = db.GetQuiz(itemId).SingleOrDefault();
                //    quiz.Attempt = db.GetCurrentQuizAttempt(itemId, UserId).SingleOrDefault();
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = quiz,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 2:
                //    var video = db.GetVideo(itemId).SingleOrDefault();
                //    video.VideoText = ItemHelper.ReplacePlaceholders(video.VideoText, UserId);
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = video,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                case 3:
                    var survey = db.GetSurvey(itemId).SingleOrDefault();
                    survey.Session = db.GetCurrentSurveySession(itemId, UserId).SingleOrDefault();
                    return Ok(new
                    {
                        ItemTypeId = item.ItemTypeId,
                        Item = survey,
                        Navigation = navigation,
                        ToolbarItems = visibleToolbarItems,
                        Actions = actions
                    });
                //case 5:
                //    var content = db.GetContent(itemId).SingleOrDefault();
                //    content.Body = ItemHelper.ReplacePlaceholders(content.Body, UserId);
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = content,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 6:
                //    var agreement = db.GetAgreement(itemId).SingleOrDefault();
                //    agreement.Body = ItemHelper.ReplacePlaceholders(agreement.Body, UserId);
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = agreement,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 7:
                //    var menu = db.GetMenu(itemId).SingleOrDefault();
                //    var menuItems = db.GetMenuItems(itemId).ToList();
                //    var visibleMenuItems = new List<GetMenuItems_Result>();
                //    menu.Body = ItemHelper.ReplacePlaceholders(menu.Body, UserId);
                //    foreach (var menuItem in menuItems)
                //    {
                //        visibleMenuItems.Add(menuItem);

                //        menuItem.ItemText = ItemHelper.ReplacePlaceholders(menuItem.ItemText, UserId);
                //        menuItem.DetailText = ItemHelper.ReplacePlaceholders(menuItem.DetailText, UserId);

                //        var itemNavigation = db.GetMenuItemNavigation(menuItem.ItemId).OrderBy(x => x.Priority).ToList();
                //        var itemLogic = db.GetMenuItemLogic(menuItem.ItemId).OrderBy(x => x.Priority).ToList();

                //        foreach (var line in itemNavigation)
                //        {
                //            if (string.IsNullOrEmpty(line.LogicExpression) || (db.EvaluateLogic(UserId, line.LogicExpression).SingleOrDefault() ?? false))
                //            {
                //                menuItem.TargetItemId = line.TargetItemId;
                //                menuItem.SelectedMenuItemId = line.SelectedMenuItemId;
                //                menuItem.NavigationTypeId = line.NavigationTypeId;
                //                menuItem.TransitionTypeId = line.TransitionTypeId;
                //                break;
                //            }
                //        }

                //        foreach (var line in itemLogic)
                //        {
                //            if (string.IsNullOrEmpty(line.Expression) || (db.EvaluateLogic(UserId, line.Expression).SingleOrDefault() ?? false))
                //            {
                //                if (line.ActionId == 1) // Hide
                //                {
                //                    visibleMenuItems.Remove(menuItem);
                //                }
                //                else if (line.ActionId == 2) // Disable
                //                {
                //                    menuItem.Disabled = true;
                //                }

                //                break;
                //            }
                //        }
                //    }
                //    menu.Items = visibleMenuItems;
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = menu,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 8:
                //    var accordion = db.GetAccordion(itemId).SingleOrDefault();
                //    accordion.Body = ItemHelper.ReplacePlaceholders(accordion.Body, UserId);
                //    accordion.Sections = db.GetAccordionSections(itemId).ToList();
                //    foreach (var section in accordion.Sections)
                //    {
                //        section.Title = ItemHelper.ReplacePlaceholders(section.Title, UserId);
                //        section.Body = ItemHelper.ReplacePlaceholders(section.Body, UserId);
                //    }
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = accordion,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 9:
                //    var resource = db.GetResource(itemId).SingleOrDefault();
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = resource,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 10:
                //    var customItem = db.GetItem(itemId).SingleOrDefault();
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = customItem,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 11:
                //    var game = db.GetMemoryGame(itemId).SingleOrDefault();
                //    game.Cards = db.GetMemoryGameCards(itemId).ToList();
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = game,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 12:
                //    var deck = db.GetDialogCardDeck(itemId).SingleOrDefault();
                //    deck.Cards = db.GetDialogCards(itemId).ToList();
                //    foreach (var card in deck.Cards)
                //    {
                //        card.FrontText = ItemHelper.ReplacePlaceholders(card.FrontText, UserId);
                //        card.BackText = ItemHelper.ReplacePlaceholders(card.BackText, UserId);
                //    }
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = deck,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 13:
                //    var form = db.GetForm(itemId).SingleOrDefault();
                //    var user = db.GetUser(UserId).SingleOrDefault();
                //    var results = db.GetFormResults(itemId, UserId, null).OrderByDescending(x => x.ResultDateUTC).ToList();
                //    GetFormResults_Result result;

                //    form.Fields = db.GetFormFields(itemId).ToList();

                //    foreach (var field in form.Fields)
                //    {
                //        field.Options = db.GetFormFieldOptions(field.FieldId).ToList();

                //        if (field.Persistent)
                //        {
                //            switch (field.FieldTypeId)
                //            {
                //                case 4:
                //                    result = results.FirstOrDefault(x => x.FieldId == field.FieldId);
                //                    if (result != null)
                //                    {
                //                        field.OptionIds = results.Where(x => x.FieldId == field.FieldId && x.ResultDateUTC == result.ResultDateUTC).Select(x => x.OptionId ?? 0)
                //                            .Where(x => x > 0).ToList();
                //                    }
                //                    break;
                //                case 16:
                //                    field.ResultText = user.FirstName;
                //                    break;
                //                case 17:
                //                    field.ResultText = user.LastName;
                //                    break;
                //                case 18:
                //                    field.ResultText = user.PhoneNumber;
                //                    break;
                //                case 19:
                //                    var notificationTypes = db.GetUserNotificationTypes(UserId).ToList();
                //                    field.OptionIds = field.Options.Where(x => notificationTypes.Any(y => y.NotificationTypeId == x.OptionValue)).Select(x => x.OptionId).ToList();
                //                    break;
                //                case 20:
                //                    field.ResultText = user.Address1;
                //                    break;
                //                case 21:
                //                    field.ResultText = user.Address2;
                //                    break;
                //                case 22:
                //                    field.ResultText = user.City;
                //                    break;
                //                case 23:
                //                    field.ResultText = user.State;
                //                    break;
                //                case 24:
                //                    field.ResultText = user.Zip;
                //                    break;
                //                case 25:
                //                    var option = field.Options.SingleOrDefault(x => x.OptionValue == user.CountryId);
                //                    if (option != null)
                //                    {
                //                        field.OptionId = option.OptionId;
                //                    }
                //                    break;
                //                case 26:
                //                    field.ResultText = user.Email;
                //                    break;
                //                default:
                //                    result = results.FirstOrDefault(x => x.FieldId == field.FieldId);
                //                    if (result != null)
                //                    {
                //                        field.OptionId = result.OptionId;
                //                        field.ResultText = result.ResultText;
                //                    }
                //                    break;
                //            }
                //        }
                //    }
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = form,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 14:
                //    var achievementItem = db.GetAchievementItem(itemId).SingleOrDefault();
                //    achievementItem.Level = db.GetUserLevel(UserId).SingleOrDefault();
                //    achievementItem.Achievements = db.GetUserAchievements(UserId).ToList();
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = achievementItem,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                //case 15:
                //    var pager = db.GetPager(itemId).SingleOrDefault();
                //    pager.Items = db.GetPagerItems(pager.PagerId).ToList();
                //    foreach (var page in pager.Items)
                //    {
                //        page.Body = ItemHelper.ReplacePlaceholders(page.Body, UserId);
                //    }
                //    return Ok(new
                //    {
                //        ItemTypeId = item.ItemTypeId,
                //        Item = pager,
                //        Navigation = navigation,
                //        ToolbarItems = visibleToolbarItems,
                //        Actions = actions
                //    });
                default:
                    break;
            }

            return Ok();
        }

        [Route("Next/{itemId?}")]
        public IHttpActionResult GetNextItem(int? itemId = null)
        {
            var navigation = db.GetItemNavigation(itemId).OrderBy(x => x.Priority).ToList();

            //if (itemId.HasValue)
            //{
            //    db.CompleteUserItem(null, itemId, UserId);
            //}

            if (navigation.Any())
            {
                foreach (var line in navigation)
                {
                    if (string.IsNullOrEmpty(line.LogicExpression) || (db.EvaluateLogic(UserId, line.LogicExpression).SingleOrDefault() ?? false))
                    {
                        return this.GetItem(line.TargetItemId, line);
                    }
                }
            }

            return Ok();
        }

        [Route("SurveyQuestion/Next/{surveyId}")]
        public IHttpActionResult GetNextSurveyQuestion(int surveyId)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();
            bool isFirstQuestion = false;

            if (session == null || session.EndDateUTC.HasValue)
            {
                var dates = GetTimeZoneAdjustedDateInfo(SystemClock.Instance.GetCurrentInstant());

                session = db.AddUpdateSurveySession(null, surveyId, UserId, dates.DateUTC, dates.DateDTO, dates.DateCTZ, null, null, null)
                    .Select(x => new GetCurrentSurveySession_Result()
                    {
                        SessionId = x.SessionId,
                        SurveyId = x.SurveyId,
                        UserId = x.UserId,
                        StartDateUTC = x.StartDateUTC,
                        EndDateUTC = x.EndDateUTC
                    }).SingleOrDefault();
            }

            var questions = db.GetSurveyQuestions(surveyId).OrderBy(x => x.QuestionOrder).ToList();
            var answers = db.GetSurveyAnswers(surveyId, UserId, session.SessionId, null, null).OrderByDescending(x => x.AnswerDateUTC).ToList();
            GetSurveyQuestions_Result question = null;

            if (answers.Count > 0)
            {
                var lastAnswer = answers.FirstOrDefault(x => questions.Any(y => y.QuestionId == x.QuestionId));

                if (lastAnswer != null)
                {
                    var lastQuestion = questions.SingleOrDefault(x => x.QuestionId == lastAnswer.QuestionId);
                    var logic = db.GetSurveyQuestionLogic(lastQuestion.QuestionId).ToList();

                    question = lastQuestion;

                    foreach (var line in logic)
                    {
                        if (line.ActionId == 2)
                        {
                            if (string.IsNullOrEmpty(line.Expression) || (db.EvaluateLogic(UserId, line.Expression).SingleOrDefault() ?? false))
                            {
                                question = questions.SingleOrDefault(x => x.QuestionId == line.ActionQuestionId);
                                break;
                            }
                        }
                        else if (line.ActionId == 3)
                        {
                            if (string.IsNullOrEmpty(line.Expression) || (db.EvaluateLogic(UserId, line.Expression).SingleOrDefault() ?? false))
                            {
                                question = null;
                                break;
                            }
                        }
                    }

                    if (question != null && question.Equals(lastQuestion))
                    {
                        question = questions.FirstOrDefault(x => x.QuestionOrder > question.QuestionOrder);
                    }

                    if (question != null)
                    {
                        question.Answer = answers.FirstOrDefault(x => x.QuestionId == question.QuestionId);

                        if (question.Answer != null)
                        {
                            question.Answers = answers.Where(x => x.QuestionId == question.QuestionId && x.AnswerDateUTC == question.Answer.AnswerDateUTC).ToList();
                        }
                    }
                }
                else
                {
                    question = questions.OrderBy(x => x.QuestionOrder).FirstOrDefault();
                    isFirstQuestion = true;
                }
            }
            else
            {
                question = questions.OrderBy(x => x.QuestionOrder).FirstOrDefault();
                isFirstQuestion = true;
            }

            if (question != null)
            {
                var logic = db.GetSurveyQuestionLogic(question.QuestionId).ToList();

                while (logic.Any())
                {
                    var lastQuestion = question;

                    foreach (var line in logic)
                    {
                        if (line.ActionId == 1)
                        {
                            if (string.IsNullOrEmpty(line.Expression) || (db.EvaluateLogic(UserId, line.Expression).SingleOrDefault() ?? false))
                            {
                                question = questions.FirstOrDefault(x => x.QuestionOrder > question.QuestionOrder);
                                break;
                            }
                        }
                    }

                    if (question != null && !question.Equals(lastQuestion))
                    {
                        logic = db.GetSurveyQuestionLogic(question.QuestionId).ToList();
                    }
                    else
                    {
                        logic.Clear();
                    }
                }
            }

            if (question == null)
            {
                var dates = GetTimeZoneAdjustedDateInfo(SystemClock.Instance.GetCurrentInstant());

                db.AddUpdateSurveySession(session.SessionId, session.SurveyId, session.UserId, session.StartDateUTC, session.StartDateDTO,
                    session.StartDateCTZ, dates.DateUTC, dates.DateDTO, dates.DateCTZ);
            }
            else
            {
                question.QuestionText = ItemHelper.ReplacePlaceholders(question.QuestionText, UserId);
                question.Body = ItemHelper.ReplacePlaceholders(question.Body, UserId);
                question.IsFirstQuestion = isFirstQuestion;
                question.Options = db.GetSurveyQuestionOptions(question.QuestionId, null).ToList();
            }

            return Ok(question);
        }

        [Route("SurveyQuestion/Previous/{surveyId}/{questionId}")]
        public IHttpActionResult GetPreviousSurveyQuestion(int surveyId, int questionId)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();
            var questions = db.GetSurveyQuestions(surveyId).OrderBy(x => x.QuestionOrder).ToList();
            var answers = db.GetSurveyAnswers(surveyId, UserId, session.SessionId, null, null).OrderByDescending(x => x.AnswerDateUTC).ToList();
            var currentQuestion = questions.SingleOrDefault(x => x.QuestionId == questionId);
            var currentAnswers = answers.Where(x => x.QuestionId == questionId).ToList();
            GetSurveyQuestions_Result question = null;

            if (answers.Count > 0)
            {
                var lastAnswer = answers[0];

                if (currentAnswers.Count > 0)
                {
                    lastAnswer = answers.FirstOrDefault(x => x.QuestionId != questionId && x.AnswerDateUTC < currentAnswers.Max(y => y.AnswerDateUTC));
                }

                var lastQuestion = questions.SingleOrDefault(x => x.QuestionId == lastAnswer.QuestionId);

                question = lastQuestion;
                question.Answer = answers.FirstOrDefault(x => x.QuestionId == question.QuestionId);

                if (question != null && question.Answer != null)
                {
                    question.Answers = answers.Where(x => x.QuestionId == question.QuestionId && x.AnswerDateUTC == question.Answer.AnswerDateUTC).ToList();
                }

                if (question != null)
                {
                    var firstAnswer = answers[answers.Count - 1];
                    var firstQuestion = questions.SingleOrDefault(x => x.QuestionId == firstAnswer.QuestionId);

                    if (firstQuestion != null)
                    {
                        question.IsFirstQuestion = question.QuestionId == firstQuestion.QuestionId;
                    }
                }
            }

            if (question != null)
            {
                question.QuestionText = ItemHelper.ReplacePlaceholders(question.QuestionText, UserId);
                question.Body = ItemHelper.ReplacePlaceholders(question.Body, UserId);
                question.Options = db.GetSurveyQuestionOptions(question.QuestionId, null).ToList();
            }

            return Ok(question);
        }

        [Route("SurveyQuestionOptions/{questionId}")]
        public IHttpActionResult GetSurveyQuestionOptions(int questionId)
        {
            var options = db.GetSurveyQuestionOptions(questionId, null);

            return Ok(options);
        }

        [HttpPost]
        [Route("SurveyAnswers/{surveyId}/{questionId}")]
        public IHttpActionResult PostSurveyAnswers(int surveyId, int questionId, List<Shared.Models.SurveyAnswer> answers)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();

            if (session.EndDateUTC.HasValue)
            {
                db.AddUpdateSurveySession(session.SessionId, session.SurveyId, session.UserId, session.StartDateUTC, session.StartDateDTO,
                    session.StartDateCTZ, null, null, null).SingleOrDefault();
            }

            var sp = new AddSurveyAnswers()
            {
                SurveyId = surveyId,
                UserId = UserId,
                SessionId = session.SessionId,
                QuestionId = questionId,
                Answers = answers
            };

            db.Database.ExecuteStoredProcedure(sp);

            return Ok();
        }

        [HttpPost]
        [Route("Survey/{surveyId}/Restart")]
        public IHttpActionResult RestartSurvey(int surveyId)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();

            if (session != null && !session.EndDateUTC.HasValue)
            {
                var dates = GetTimeZoneAdjustedDateInfo(SystemClock.Instance.GetCurrentInstant());

                db.AddUpdateSurveySession(session.SessionId, session.SurveyId, session.UserId, session.StartDateUTC, session.StartDateDTO,
                    session.StartDateCTZ, dates.DateUTC, dates.DateDTO, dates.DateCTZ);
            }

            return Ok();
        }

        [HttpPost]
        [Route("SurveyImage/{surveyId}/{questionId}")]
        public async Task<IHttpActionResult> PostSurveyImage(int surveyId, int questionId)
        {
            var session = db.GetCurrentSurveySession(surveyId, UserId).SingleOrDefault();
            var answers = new List<Shared.Models.SurveyAnswer>();
            string root = HostingEnvironment.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            await Request.Content.ReadAsMultipartAsync(provider);

            string localPath = HostingEnvironment.MapPath("~/Images");

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            var answer = JsonConvert.DeserializeObject<Shared.Models.SurveyAnswer>(provider.FormData.GetValues("answer")[0]);

            foreach (var file in provider.FileData)
            {
                string fileName = string.Format("{0}_{1}.jpg", Guid.NewGuid().ToString().Replace("-", string.Empty), DateTime.Now.ToString("MMddyyyy"));
                File.Move(file.LocalFileName, Path.Combine(localPath, fileName));
                answer.AnswerText = fileName;
                answers.Add(answer);
            }

            var sp = new AddSurveyAnswers()
            {
                SurveyId = surveyId,
                UserId = UserId,
                SessionId = session.SessionId,
                QuestionId = questionId,
                Answers = answers
            };

            db.Database.ExecuteStoredProcedure(sp);

            return Ok();
        }
    }
}