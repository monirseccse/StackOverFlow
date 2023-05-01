using Autofac;
using StackOverFlowClone.Web.Areas.App.Models.Answers;
using StackOverFlowClone.Web.Areas.App.Models.Questions;
using StackOverFlowClone.Web.Models;

namespace StackOverFlowClone.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<AccountEditModel>().AsSelf();
            builder.RegisterType<QuestionCreateModel>().AsSelf();
            builder.RegisterType<QuestionListModel>().AsSelf();
            builder.RegisterType<AnswerCreateModel>().AsSelf();
            base.Load(builder);
        }
    }
}
