using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public interface IQuestionRepository : IRepository<Question, Guid>
    {
    }
}
