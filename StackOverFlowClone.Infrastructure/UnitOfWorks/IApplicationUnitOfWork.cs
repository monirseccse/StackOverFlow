using StackOverFlowClone.Infrastructure.Repositories;

namespace StackOverFlowClone.Infrastructure.UnitOfWorks
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IQuestionRepository Questions { get; }
        public IAnswerRepository Answer { get; }
        public IVoteForAnswerRepository VoteForAnswer { get; }
        public IVoteForQuestionRepository VoteForQuestion { get; }
    }
}
