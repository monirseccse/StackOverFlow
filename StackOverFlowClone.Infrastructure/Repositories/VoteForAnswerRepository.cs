using NHibernate;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public class VoteForAnswerRepository : Repository<VoteForAnswer, Guid>, IVoteForAnswerRepository
    {
        public VoteForAnswerRepository(ISession session) : base(session)
        {
        }
    }
}
