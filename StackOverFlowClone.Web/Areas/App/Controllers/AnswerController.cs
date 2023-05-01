using Autofac;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using StackOverFlowClone.Infrastructure.Exceptions;
using StackOverFlowClone.Web.Areas.App.Models.Answers;
using StackOverFlowClone.Web.Areas.App.Models.Questions;
using StackOverFlowClone.Web.Codes;
using StackOverFlowClone.Web.Models;
using System.Reflection;

namespace StackOverFlowClone.Web.Areas.App.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly ILifetimeScope _scope;
        public AnswerController(
            ILogger<AnswerController> logger,
            ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = _scope.Resolve<AnswerCreateModel>();
            model.QuestionId = id;
            await model.LoadAnswerAsync(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);
                try
                {
                    await model.CreateAsync();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully created Answer " ,
                        Type = ResponseTypes.Success
                    });

                    RedirectToAction("Asked", "Question", new { Area = "App" });
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
    }
}
