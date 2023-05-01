using AutoMapper;
using StackOverFlowClone.Infrastructure.BusinessObjects;
using StackOverFlowClone.Web.Areas.App.Models.Answers;
using StackOverFlowClone.Web.Areas.App.Models.Questions;

namespace StackOverFlowClone.Web.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<QuestionCreateModel, Question>()
                .ReverseMap();

            CreateMap<AnswerCreateModel, Answer>()
                .ReverseMap();
        }
    }
}
