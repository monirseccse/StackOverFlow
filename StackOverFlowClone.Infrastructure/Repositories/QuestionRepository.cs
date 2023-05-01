using NHibernate;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(ISession session) : base(session)
        {
        }
    }
}
