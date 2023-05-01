using AutoMapper;
using StackOverFlowClone.Infrastructure.BusinessObjects;
using StackOverFlowClone.Infrastructure.Entities;
using StackOverFlowClone.Infrastructure.UnitOfWorks;
using VoteForAnswerBO = StackOverFlowClone.Infrastructure.BusinessObjects.VoteForAnswer;
using VoteForAnswerEO = StackOverFlowClone.Infrastructure.Entities.VoteForAnswer;

namespace StackOverFlowClone.Infrastructure.Services
{
    public class VoteForAnswerService : IVoteForAnswerService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public VoteForAnswerService(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork; ;
            _mapper = mapper;
        }

        public async Task CreateVoteAsync(VoteForAnswerBO vote)
        {
            var votes = await Task.Run(()=> _applicationUnitOfWork.VoteForAnswer.FindBy
                (x => x.UserId == vote.UserId && x.AnswerId == vote.AnswerId));

            if(votes.Count() > 0)
            {
                if(votes.Count() == 1 && vote.isUpVote != votes.FirstOrDefault().isUpVote)
                {
                   await EditVoteAsync(vote);
                }
                else
                throw new InvalidOperationException("Vote Already exists");
            }
            else
            {
                var entity = _mapper.Map<VoteForAnswerEO>(vote);
                _applicationUnitOfWork.VoteForAnswer.Add(entity);
                _applicationUnitOfWork.SaveChanges();

                UpdateAnswer(entity, vote);
            }
        }

        private void UpdateAnswer(VoteForAnswerEO entity, VoteForAnswerBO vote)
        {
            var answer = _applicationUnitOfWork.Answer.FindBy(entity.AnswerId);

            answer.Vote = vote.isUpVote ? answer.Vote + 1 : answer.Vote - 1;
            _applicationUnitOfWork.Answer.Update(answer);
            _applicationUnitOfWork.SaveChanges();
        }

        public Task DeleteVoteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task EditVoteAsync(VoteForAnswerBO vote)
        {
            var entity = await Task.Run(() => _applicationUnitOfWork.VoteForAnswer.FindBy(
                           x => x.AnswerId == vote.AnswerId &&
                           x.UserId == vote.UserId).FirstOrDefault());

            vote.Id = entity.Id;

            _mapper.Map(vote,entity);
            _applicationUnitOfWork.VoteForAnswer.Update(entity);
            _applicationUnitOfWork.SaveChanges();

            UpdateAnswer(entity, vote);
        }

        public async Task<VoteForAnswerBO> GetVoteDetail(Guid answerId, Guid userId)
        {
            var entity = await Task.Run(()=>_applicationUnitOfWork.VoteForAnswer.FindBy(
                x=>x.AnswerId == answerId && x.UserId == userId).FirstOrDefault());
            
            VoteForAnswerBO answerBO = new VoteForAnswerBO();
            _mapper.Map(entity, answerBO);

            return answerBO;
        }

        public async Task<VoteForAnswerBO> GetVoteDetailByAnswerId(Guid answerId)
        {
            var entity = await Task.Run(() => _applicationUnitOfWork.VoteForAnswer.FindBy(
                 x => x.AnswerId == answerId).FirstOrDefault());

            VoteForAnswerBO answerBO = new VoteForAnswerBO();
            _mapper.Map(entity, answerBO);

            return answerBO;
        }
    }
}
