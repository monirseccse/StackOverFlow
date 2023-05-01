using NHibernate;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public class AnswerRepository : Repository<Answer, Guid>, IAnswerRepository
    {
        public AnswerRepository(ISession session) : base(session)
        {
        }
    }
}
