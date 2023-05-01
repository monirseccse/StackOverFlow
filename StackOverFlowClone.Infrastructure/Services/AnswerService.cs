using AutoMapper;
using FluentNHibernate.Data;
using StackOverFlowClone.Infrastructure.UnitOfWorks;
using AnswerBO = StackOverFlowClone.Infrastructure.BusinessObjects.Answer;
using AnswerEO = StackOverFlowClone.Infrastructure.Entities.Answer;

namespace StackOverFlowClone.Infrastructure.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public AnswerService(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork; ;
            _mapper = mapper;
        }

        public async Task CreateAnswerAsync(AnswerBO answer, Guid userId)
        {
            var entity = _mapper.Map<AnswerEO>(answer);
            entity.UserId = userId;

            _applicationUnitOfWork.Answer.Add(entity);
            _applicationUnitOfWork.SaveChanges();

            var question = _applicationUnitOfWork.Questions.FindBy(answer.QuestionId);
            question.Answer = question.Answer + 1;

            _applicationUnitOfWork.Questions.Update(question);
            _applicationUnitOfWork.SaveChanges();
        }

        public async Task DeleteAnswerAsync(Guid id)
        {
            var result = _applicationUnitOfWork.Answer.DeleteById(id);
            _applicationUnitOfWork.SaveChanges();
        }

        public async Task EditAnswerAsync(AnswerBO answer)
        {
           var entity =_applicationUnitOfWork.Answer.FindBy(answer.Id);

            if(entity == null)
                throw new InvalidOperationException("Answer was not found");

            _mapper.Map(answer, entity);
            var result =  _applicationUnitOfWork.Answer.Update(entity);
            _applicationUnitOfWork.SaveChanges();
        }

        public async Task<AnswerBO> GetAnswerAsync(Guid id)
        {
            var answer = _applicationUnitOfWork.Answer.FindBy(id);
            var ansbo = _mapper.Map<AnswerBO>(answer);

            return ansbo;
        }

        public async Task<IList<AnswerBO>> GetAnswerByQuestionIdAsync(Guid id)
        {
            var answer = await Task.Run(() => _applicationUnitOfWork.Answer.FindBy(x => x.QuestionId == id));
            var ansbo = answer.Select(x=> _mapper.Map<AnswerBO>(x));
            return ansbo.ToList();
        }

        public async Task<(IList<AnswerBO> records, int total, int totalDisplay)> GetAnswers(int pageIndex, int pageSize, string searchText, string orderBy)
        {
            throw new NotImplementedException();
        }

    }
}
