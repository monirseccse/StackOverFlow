using Autofac;
using StackOverFlowClone.Infrastructure.Repositories;
using StackOverFlowClone.Infrastructure.Services;
using StackOverFlowClone.Infrastructure.Services.Encryption;
using StackOverFlowClone.Infrastructure.UnitOfWorks;

namespace StackOverFlowClone.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>()
                 .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
               .InstancePerLifetimeScope();

            builder.RegisterType<QuestionService>().As<IQuestionService>()
              .InstancePerLifetimeScope();

            builder.RegisterType<AnswerService>().As<IAnswerService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<AnswerRepository>().As<IAnswerRepository>()
             .InstancePerLifetimeScope();

            builder.RegisterType<VoteForAnswerRepository>().As<IVoteForAnswerRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<VoteForAnswerService>().As<IVoteForAnswerService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<VoteForQuestionService>().As<IVoteForQuestionService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<VoteForQuestionRepository>().As<IVoteForQuestionRepository>()
             .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
