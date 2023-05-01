using NHibernate;
using StackOverFlowClone.Infrastructure.Repositories;

namespace StackOverFlowClone.Infrastructure.UnitOfWorks
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(
            ISession session,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepository,
            IVoteForAnswerRepository voteForAnswerRepository,
            IVoteForQuestionRepository voteForQuestion
            ) : base(session)
        {
            Questions = questionRepository;
            Answer = answerRepository;
            VoteForAnswer = voteForAnswerRepository;
            VoteForQuestion = voteForQuestion;
        }

        public IQuestionRepository Questions { get; private set; }
        public IAnswerRepository Answer { get; private set; }
        public IVoteForAnswerRepository VoteForAnswer { get;private set; }
        public IVoteForQuestionRepository VoteForQuestion { get; private set; }
    }
}
