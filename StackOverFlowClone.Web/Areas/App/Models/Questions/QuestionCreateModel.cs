using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using StackOverFlowClone.Infrastructure.BusinessObjects;
using StackOverFlowClone.Infrastructure.Services;

namespace StackOverFlowClone.Web.Areas.App.Models.Questions
{
    public class QuestionCreateModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TriedAndExpecting { get; set; }
        public string Tag { get; set; }
        public string? DuplicateQuestion { get; set; }
        public int Vote { get; set; }
        public int Answer { get; set; }
        public Guid UserId { get; set; }

        private IQuestionService _questionService;
        private IVoteForQuestionService _voteForQuestionService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;

        public QuestionCreateModel()
        {

        }

        public QuestionCreateModel(IQuestionService questionService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IVoteForQuestionService voteForQuestionService)
        {
            _questionService = questionService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _voteForQuestionService = voteForQuestionService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _questionService = _scope.Resolve<IQuestionService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task CreateAsync()
        {
            var currentUserId =  GetLoggedInUserId();

            if (currentUserId != Guid.Empty)
            {

                var question = _mapper.Map<Question>(this);

                await _questionService.CreateQuestionAsync(question, currentUserId);
            }

        }

        private  Guid GetLoggedInUserId()
        {
            var currentuser = _httpContextAccessor.HttpContext.User
                .Claims.FirstOrDefault();

            Guid currentUserId = Guid.Empty;
            if (currentuser != null)
            {
                currentUserId = Guid.Parse(currentuser.Value);
            }

            return  currentUserId;
        }

        public async Task<bool> isNotAlreadyDownVoted()
        {
            var vote = await _voteForQuestionService.GetVoteDetail(Id, GetLoggedInUserId());

            return vote == null ? false : !vote.isDownVote;
        }

        public async Task DownVote()
        {
            VoteForQuestion voteForAnswer = new VoteForQuestion();
            voteForAnswer.isUpVote = false;
            voteForAnswer.QuestionId = Id;
            voteForAnswer.UserId = GetLoggedInUserId();
            voteForAnswer.isDownVote = true;

            await _voteForQuestionService.CreateVoteAsync(voteForAnswer);
        }

        public async Task LoadQuestionAsync(Guid questionId)
        {
            var question = await _questionService.GetQuestionAsync(questionId);
            _mapper.Map(question, this);
        }

        public async Task EditAsync(QuestionCreateModel model)
        {
            var result = _mapper.Map<Question>(model);
            await _questionService.EditQuestion(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _questionService.DeleteQuestionAsync(id);
        }

        public async Task<bool> IsAuthorized()
        {
            return UserId == GetLoggedInUserId();
        }

        public async Task<bool> isNotAlreadyUpVoted()
        {
            var vote = await _voteForQuestionService.GetVoteDetail(Id, GetLoggedInUserId());

            return vote == null ? false : !vote.isUpVote;
        }

        public async Task UpVote()
        {
            VoteForQuestion voteForQuestion = new VoteForQuestion();
            voteForQuestion.isUpVote = true;
            voteForQuestion.QuestionId = Id;
            voteForQuestion.UserId = GetLoggedInUserId();
            voteForQuestion.isDownVote = false;

            await _voteForQuestionService.CreateVoteAsync(voteForQuestion);
        }

        public async Task<bool> isNotOwner()
        {
            return UserId != GetLoggedInUserId();
        }
    }
}
