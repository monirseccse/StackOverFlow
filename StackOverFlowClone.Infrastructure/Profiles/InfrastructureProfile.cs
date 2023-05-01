using AutoMapper;
using AnswerBO = StackOverFlowClone.Infrastructure.BusinessObjects.Answer;
using AnswerEO = StackOverFlowClone.Infrastructure.Entities.Answer;
using QuestionBO = StackOverFlowClone.Infrastructure.BusinessObjects.Question;
using QuestionEO = StackOverFlowClone.Infrastructure.Entities.Question;
using VoteForAnswerBO = StackOverFlowClone.Infrastructure.BusinessObjects.VoteForAnswer;
using VoteForAnswerEO = StackOverFlowClone.Infrastructure.Entities.VoteForAnswer;
using VoteForQuestionBO = StackOverFlowClone.Infrastructure.BusinessObjects.VoteForQuestion;
using VoteForQuestionEO = StackOverFlowClone.Infrastructure.Entities.VoteForQuestion;


namespace DevTrack.Infrastructure.Profiles
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<QuestionBO, QuestionEO>()
                .ReverseMap();

            CreateMap<AnswerBO, AnswerEO>()
                .ReverseMap();

            CreateMap<VoteForAnswerBO, VoteForAnswerEO>()
                .ReverseMap();

            CreateMap<VoteForQuestionBO, VoteForQuestionEO>()
                .ReverseMap();
        }
    }
}
