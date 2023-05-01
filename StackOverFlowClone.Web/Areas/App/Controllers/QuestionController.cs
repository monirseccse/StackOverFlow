using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlowClone.Infrastructure.Exceptions;
using StackOverFlowClone.Web.Areas.App.Models.Answers;
using StackOverFlowClone.Web.Areas.App.Models.Questions;
using StackOverFlowClone.Web.Codes;
using StackOverFlowClone.Web.Models;

namespace StackOverFlowClone.Web.Areas.App.Controllers
{
    [Area("App")]
    public class QuestionController : Controller
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly ILifetimeScope _scope;
        public QuestionController(
            ILogger<QuestionController> logger,
            ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Asked(Guid questionId)
        {
            var model = _scope.Resolve<QuestionCreateModel>();
            model.Id = questionId;

            await model.LoadQuestionAsync(questionId);

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var model = _scope.Resolve<QuestionCreateModel>();
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                try
                {
                    await model.CreateAsync();

                    TempData["QuestionTitle"] = model.Title;
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully created question - " + model.Title,
                        Type = ResponseTypes.Success
                    });

                    RedirectToAction(nameof(Asked));
                }
                catch (DuplicateException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);
                    ModelState.AddModelError("", ioe.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });

                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in creating Question.",
                        Type = ResponseTypes.Danger
                    });

                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }


        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = _scope.Resolve<QuestionCreateModel>();

            await model.LoadQuestionAsync(id);

            if (await model.IsAuthorized())
            {
                return View(model);
            }
            else
            {
                return BadRequest("You are not Authorized");
            }


        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionCreateModel model)
        {
            var resolve = _scope.Resolve<QuestionCreateModel>();
            try
            {
                await resolve.EditAsync(model);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Question Updated Successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (DuplicateException ioe)
            {
                _logger.LogError(ioe, ioe.Message);
                ModelState.AddModelError("", ioe.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message,
                    Type = ResponseTypes.Danger
                });

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in updating project.");
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in updating project.",
                    Type = ResponseTypes.Danger
                });

                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> QuestionUpVote(Guid id)
        {
            var model = _scope.Resolve<QuestionCreateModel>();

            await model.LoadQuestionAsync(id);

            if (await model.isNotAlreadyUpVoted())
            {
                if(await model.isNotOwner())
                {
                    await model.UpVote();
                }
                else
                {
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "owner can't upvote",
                        Type = ResponseTypes.Success
                    });
                }
            }
            else
            {
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Vote already exists",
                    Type = ResponseTypes.Success
                });
            }
           

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> QuestionDownVote(Guid id)
        {
            var model = _scope.Resolve<QuestionCreateModel>();

            await model.LoadQuestionAsync(id);

            if (await model.isNotAlreadyDownVoted())
            {
                if (await model.isNotOwner())
                {
                    await model.DownVote();
                }
                else
                {

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Owner can't downvote",
                        Type = ResponseTypes.Success
                    });
                }
               
            }
            else
            {

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Vote already exists",
                    Type = ResponseTypes.Success
                });
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = _scope.Resolve<QuestionCreateModel>();
            await model.LoadQuestionAsync(id);

            if (await model.IsAuthorized())
            {
                await model.DeleteAsync(id);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfully deleted question.",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("You are not Authorized");
            }
        }

        public async Task<JsonResult> GetQuestionData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<QuestionListModel>();

            return Json(await model.GetPagedQuestions(dataTableModel));
        }

        [Authorize]
        public async Task<IActionResult> AnswerPost(Guid id)
        {
            var model = _scope.Resolve<AnswerCreateModel>();
            model.QuestionId = id;

            return View(model);
        }

        public async Task<JsonResult> GetAnswerList(Guid questionId)
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<AnswerCreateModel>();

            return Json(await model.PagedAnswers(questionId));

        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnswer(Guid id, AnswerCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                try
                {
                    model.QuestionId = id;
                    await model.CreateAsync();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully created Answer ",
                        Type = ResponseTypes.Success
                    });

                    RedirectToAction("Asked");
                }
                catch (DuplicateException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);
                    ModelState.AddModelError("", ioe.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Asked");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in creating Question.",
                        Type = ResponseTypes.Danger
                    });

                    return RedirectToAction("Asked");
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> EditAnswer(Guid answerId)
        {
            var model = _scope.Resolve<AnswerCreateModel>();

            await model.LoadAnswerAsync(answerId);

            if (await model.IsAuthorized())
            {
                return View(model);
            }
            else
            {
                return BadRequest("You are not Authorized");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(AnswerCreateModel model)
        {

            model.ResolveDependency(_scope);
            try
            {
                await model.EditAsync();

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Answer Updated Successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (DuplicateException ioe)
            {
                _logger.LogError(ioe, ioe.Message);
                ModelState.AddModelError("", ioe.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message,
                    Type = ResponseTypes.Danger
                });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in updating project.");
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in updating project.",
                    Type = ResponseTypes.Danger
                });
            }

            return View(model);

        }

        [Authorize]
        public async Task<IActionResult> DeleteAnswer(Guid answerId)
        {
            var model = _scope.Resolve<AnswerCreateModel>();

            model.LoadAnswerAsync(answerId);

            if (await model.IsAuthorized())
            {
                await model.DeleteAsync(answerId);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfully deleted question.",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Asked");
            }
            else
            {
                return BadRequest("You are not Authorized");
            }
        }

        [Authorize]
        public async Task<IActionResult> UpVoteAnswer(Guid answerId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = _scope.Resolve<AnswerCreateModel>();

                await model.LoadAnswerAsync(answerId);

                if (await model.isNotAlreadyUpVoted() )
                {
                    if(await model.isNotTheOwner())
                    {
                        return View(model);
                    }
                    else
                    {

                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "You can't Vote on your answer",
                            Type = ResponseTypes.Success
                        });
                    }
                }

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Vote already recorded",
                    Type = ResponseTypes.Success
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpVoteAnswer(AnswerCreateModel model)
        {
            try
            {
                model.ResolveDependency(_scope);
                await model.UpVote();

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Vote recored Successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "error Occured",
                    Type = ResponseTypes.Success
                });

                return View(model);
            }

        }

        [Authorize]
        public async Task<IActionResult> DownVoteAnswer(Guid answerId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = _scope.Resolve<AnswerCreateModel>();

                await model.LoadAnswerAsync(answerId);

                if (await model.isNotAlreadyDownVoted())
                {
                    if (await model.isNotTheOwner())
                    {
                        return View(model);
                    }
                    else
                    {

                        TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                        {
                            Message = "You can't Vote on your answer",
                            Type = ResponseTypes.Success
                        });
                    }
                }
                else
                {

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Vote already recorded",
                        Type = ResponseTypes.Success
                    });
                }

            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DownVoteAnswer(AnswerCreateModel model)
        {
            try
            {
                model.ResolveDependency(_scope);
                await model.DownVote();

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Vote recored Successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "error Occured",
                    Type = ResponseTypes.Success
                });

                return View(model);
            }

        }

        [Authorize]
        public async Task<IActionResult> AcceptAnswer(Guid answerId)
        {
            var model = _scope.Resolve<AnswerCreateModel>();

            await model.LoadAnswerAsync(answerId);

            if (await model.IsOwnerAuthorized())
            {
                return View(model);
            }
            else
            {
                return BadRequest("You are not Authorized");
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAnswer(AnswerCreateModel model)
        {
            try
            {
                model.ResolveDependency(_scope);

                model.IsAccepted = true;
                await model.EditAsync();

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Answer Accepted Successfully",
                    Type = ResponseTypes.Success
                });

                return RedirectToAction("Index");
            }
            catch (DuplicateException ioe)
            {
                _logger.LogError(ioe, ioe.Message);
                ModelState.AddModelError("", ioe.Message);
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = ioe.Message,
                    Type = ResponseTypes.Danger
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in updating project.");
                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in updating project.",
                    Type = ResponseTypes.Danger
                });


            }

            return View(model);
        }


    }
}
