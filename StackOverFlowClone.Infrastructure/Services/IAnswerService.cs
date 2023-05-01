using StackOverFlowClone.Infrastructure.BusinessObjects;

namespace StackOverFlowClone.Infrastructure.Services
{
    public interface IAnswerService
    {
        Task CreateAnswerAsync(Answer answer, Guid userId);
        Task<Answer> GetAnswerAsync(Guid id);
        Task DeleteAnswerAsync(Guid id);
        Task EditAnswerAsync(Answer answer);
        Task<IList<Answer>> GetAnswerByQuestionIdAsync(Guid id);
        Task<(IList<Answer> records, int total, int totalDisplay)> GetAnswers(int pageIndex,
            int pageSize, string searchText, string orderBy);
    }
}
