using Autofac;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StackOverFlowClone.Infrastructure.BusinessObjects;
using StackOverFlowClone.Infrastructure.Services;
using System.Reflection;

namespace StackOverFlowClone.Web.Areas.App.Models.Answers
{
    public class AnswerCreateModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Vote { get; set; }
        public bool IsAccepted { get; set; }
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }

        private IAnswerService _AnswerService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;
        private IQuestionService _questionService;
        private IVoteForAnswerService _voteForAnswerService;

        public AnswerCreateModel()
        {

        }

        public AnswerCreateModel(
            IAnswerService answerService,
            IQuestionService questionService,
            IVoteForAnswerService voteForAnswerService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _AnswerService = answerService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _questionService = questionService;
            _voteForAnswerService= voteForAnswerService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _AnswerService = _scope.Resolve<IAnswerService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _voteForAnswerService = _scope.Resolve<IVoteForAnswerService>();
        }

        public async Task CreateAsync()
        {
            var currentUserId = GetLoggedInUserId();

            if (currentUserId != Guid.Empty)
            {

                var question = _mapper.Map<Answer>(this);

                await _AnswerService.CreateAnswerAsync(question, currentUserId);
            }

        }

        private Guid GetLoggedInUserId()
        {
            var currentuser = _httpContextAccessor.HttpContext.User
                .Claims.FirstOrDefault();

            Guid currentUserId = Guid.Empty;
            if (currentuser != null)
            {
                currentUserId = Guid.Parse(currentuser.Value);
            }

            return currentUserId;
        }

        public async Task<IList<Answer>> GetAnswerlistByQuestionId(Guid questionId)
        {
            return await _AnswerService.GetAnswerByQuestionIdAsync(questionId);
        }

        public async Task<object?> PagedAnswers(Guid questionId)
        {
            var data = await _AnswerService.GetAnswerByQuestionIdAsync(questionId);

            return new
            {
                recordsTotal = data.Count(),
                recordsFiltered = data.Count(),
                data = (from record in data
                        select new string[]
                        {
                            record.IsAccepted.ToString(),
                             record.Vote.ToString(),
                            record.Vote.ToString(),
                            record.Description.ToString(),
                            record.Id.ToString()
                        }
                   ).ToArray()
            };
        }

        public async Task LoadAnswerAsync(Guid answerId)
        {
            var answer = await _AnswerService.GetAnswerAsync(answerId);
            _mapper.Map(answer, this);
        }

        public async Task<bool> IsAuthorized()
        {
            return UserId == GetLoggedInUserId();
        }

        public async Task EditAsync()
        {
            var result = _mapper.Map<Answer>(this);
            await _AnswerService.EditAnswerAsync(result);
        }

        public async Task DeleteAsync(Guid answerId)
        {
            await _AnswerService.DeleteAnswerAsync(answerId);
        }

        public async Task<bool> IsOwnerAuthorized()
        {
            var question = await _questionService.GetQuestionAsync(QuestionId);
            
            if (question == null)
                return false;

            return question.UserId == UserId;
        }

        public async Task<object?> GetAnswerById(Guid answerId)
        {
            var answer = await _AnswerService.GetAnswerAsync(answerId);

            if (answer == null)
            {
                throw new InvalidOperationException("Answer not found");
               
            }

            answer.Vote += 1;
            await _AnswerService.EditAnswerAsync(answer);

            return answer;
        }

        public async Task<bool> isNotAlreadyUpVoted()
        {
            var vote = await  _voteForAnswerService.GetVoteDetail(Id, GetLoggedInUserId());
            
            return vote == null ? false : !vote.isUpVote;
        }

        public async Task UpVote()
        {
            VoteForAnswer voteForAnswer = new VoteForAnswer();
            voteForAnswer.isUpVote = true;
            voteForAnswer.AnswerId = Id;
            voteForAnswer.UserId = GetLoggedInUserId();
            voteForAnswer.isDownVote= false;

            await _voteForAnswerService.CreateVoteAsync(voteForAnswer);
        }

        public async Task DownVote()
        {
            VoteForAnswer voteForAnswer = new VoteForAnswer();
            voteForAnswer.isUpVote = false;
            voteForAnswer.AnswerId = Id;
            voteForAnswer.UserId = GetLoggedInUserId();
            voteForAnswer.isDownVote = true;

            await _voteForAnswerService.CreateVoteAsync(voteForAnswer);
        }

        public async Task<bool> isNotAlreadyDownVoted()
        {
            var vote = await _voteForAnswerService.GetVoteDetail(Id, GetLoggedInUserId());

            return vote == null ? false : !vote.isDownVote;
        }

        public async Task<bool> isNotTheOwner()
        {
            return UserId != GetLoggedInUserId();
        }
    }
}
