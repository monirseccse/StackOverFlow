using StackOverFlowClone.Infrastructure.BusinessObjects;

namespace StackOverFlowClone.Infrastructure.Services
{
    public interface IQuestionService
    {
        Task CreateQuestionAsync(Question question, Guid userId);
        Task<Question> GetQuestionAsync(Guid id);
        Task DeleteQuestionAsync(Guid id);
        Task<IList<Question>> GetAllQuestionsAsync();
        Task<IList<Question>> GetUserQuestionsAsync(Guid id);
        Task<(IList<Question> records, int total, int totalDisplay)> GetQuestions(int pageIndex,
             int pageSize, string searchText, string orderBy);
        Task EditQuestion(Question Question);
        Task CreateQuestionUserAsync(Question Question, Guid userId);
    }
}
