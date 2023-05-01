using AutoMapper;
using VoteForQuestionBO = StackOverFlowClone.Infrastructure.BusinessObjects.VoteForQuestion;
using VoteForQuestionEO = StackOverFlowClone.Infrastructure.Entities.VoteForQuestion;
using StackOverFlowClone.Infrastructure.UnitOfWorks;

namespace StackOverFlowClone.Infrastructure.Services
{
    public class VoteForQuestionService : IVoteForQuestionService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public VoteForQuestionService(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork; ;
            _mapper = mapper;
        }
        public async Task CreateVoteAsync(VoteForQuestionBO vote)
        {
            var votes = await Task.Run(() => _applicationUnitOfWork.VoteForQuestion.FindBy
                  (x => x.UserId == vote.UserId && x.QuestionId == vote.QuestionId));

            if (votes.Count() > 0)
            {
                if (votes.Count() == 1 && vote.isUpVote != votes.FirstOrDefault().isUpVote)
                {
                    await EditVoteAsync(vote);
                }
                else
                    throw new InvalidOperationException("Vote Already exists");
            }

            var entity = _mapper.Map<VoteForQuestionEO>(vote);
            _applicationUnitOfWork.VoteForQuestion.Add(entity);
            _applicationUnitOfWork.SaveChanges();

            var answer = _applicationUnitOfWork.Questions.FindBy(entity.QuestionId);

            answer.Vote = vote.isUpVote ? answer.Vote + 1 : answer.Vote - 1;
            _applicationUnitOfWork.Questions.Update(answer);
            _applicationUnitOfWork.SaveChanges();
        }

        public Task DeleteVoteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task EditVoteAsync(VoteForQuestionBO vote)
        {
            var entity = await Task.Run(() => _applicationUnitOfWork.VoteForQuestion.FindBy(
                           x => x.QuestionId == vote.QuestionId &&
                           x.UserId == vote.UserId).FirstOrDefault());

            vote.Id = entity.Id;

            _mapper.Map(vote, entity);
            _applicationUnitOfWork.VoteForQuestion.Update(entity);
            _applicationUnitOfWork.SaveChanges();
        }

        public async Task<VoteForQuestionBO> GetVoteDetail(Guid questionId, Guid userId)
        {
            var entity = await Task.Run(() => _applicationUnitOfWork.VoteForQuestion.FindBy(
                x => x.QuestionId == questionId && x.UserId == userId).FirstOrDefault());

            VoteForQuestionBO answerBO = new VoteForQuestionBO();
            _mapper.Map(entity, answerBO);

            return answerBO;
        }

        public async Task<VoteForQuestionBO> GetVoteDetailByQuestionId(Guid questionId)
        {
            var entity = await Task.Run(() => _applicationUnitOfWork.VoteForQuestion.FindBy(
                 x => x.QuestionId == questionId).FirstOrDefault());

            VoteForQuestionBO answerBO = new VoteForQuestionBO();
            _mapper.Map(entity, answerBO);

            return answerBO;
        }
    }
}
