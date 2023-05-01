using AutoMapper;
using StackOverFlowClone.Infrastructure.Exceptions;
using StackOverFlowClone.Infrastructure.UnitOfWorks;
using QuestionBO = StackOverFlowClone.Infrastructure.BusinessObjects.Question;
using QuestionEO = StackOverFlowClone.Infrastructure.Entities.Question;

namespace StackOverFlowClone.Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork; ;
            _mapper = mapper;
        }

        public async Task CreateQuestionAsync(QuestionBO question, Guid userId)
        {
            var count =  _applicationUnitOfWork.Questions.FindBy(x => x.Title.ToLower() ==
                question.Title.ToLower() && x.Tag.ToLower() == question.Tag).Count();

            if (count > 0)
            {
                throw new DuplicateException($"This is Question Already exists within {question.Tag} Tag");
            }

            var entity = _mapper.Map<QuestionEO>(question);
            entity.UserId = userId;

            _applicationUnitOfWork.Questions.Add(entity);
            _applicationUnitOfWork.SaveChanges();
        }

        public Task CreateQuestionUserAsync(QuestionBO Question, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task EditQuestion(QuestionBO question)
        {
            var entity = _mapper.Map<QuestionEO>(question);
            var id = _applicationUnitOfWork.Questions.Update(entity);
            _applicationUnitOfWork.SaveChanges();
        }

        public Task<IList<QuestionBO>> GetAllQuestionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionBO> GetQuestionAsync(Guid id)
        {
            var questionEO = _applicationUnitOfWork.Questions.FindBy(id);

            if(questionEO == null)
            {
                throw new InvalidOperationException("Question not found");
            }

            var questionBO = _mapper.Map<QuestionBO>(questionEO);

            return questionBO;
        }

        public async Task DeleteQuestionAsync(Guid id)
        {
            var questionEO = _applicationUnitOfWork.Questions.FindBy(id);

            if (questionEO != null)
            {
                _applicationUnitOfWork.Questions.Delete(questionEO);
                _applicationUnitOfWork.SaveChanges();
            }
        }

        public Task<IList<QuestionBO>> GetUserQuestionsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<(IList<QuestionBO> records, int total, int totalDisplay)> GetQuestions(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            var result = await _applicationUnitOfWork.Questions.GetDynamicAsync(x => x.Title.Contains(searchText), orderBy,
                pageIndex, pageSize);

            var questions = result.data.Select(x => _mapper.Map<QuestionBO>(x)).ToList();

            return (questions, result.total, result.totalDisplay);
        }
    }
}
