using Autofac;
using AutoMapper;
using StackOverFlowClone.Infrastructure.Services;
using StackOverFlowClone.Web.Models;

namespace StackOverFlowClone.Web.Areas.App.Models.Questions
{
    public class QuestionListModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TriedAndExpecting { get; set; }
        public string Tag { get; set; }
        public string DuplicateQuestion { get; set; }
        public int Votes { get; set; }
        public int Answer { get; set; }

        private IQuestionService _questionService;
        private  IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;

        public QuestionListModel()
        {

        }

        public QuestionListModel(IQuestionService questionService,
            IMapper mapper, IHttpContextAccessor? httpContextAccessor)
        {
            _questionService = questionService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _questionService = _scope.Resolve<IQuestionService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task<object?> GetPagedQuestions(DataTablesAjaxRequestModel model)
        {
            var data = await _questionService.GetQuestions(
                model.PageIndex,
                model.PageSize,
                model.SearchText,
               // model.GetSortText(new string[] { "Title"})
                "");

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Title,
                            record.Tag.ToString(),
                            record.Vote.ToString(),
                            record.Answer.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
       
    }
}
