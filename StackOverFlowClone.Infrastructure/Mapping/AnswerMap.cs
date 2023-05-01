using FluentNHibernate.Mapping;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Mapping
{
    public class AnswerMap : ClassMap<Answer>
    {
        public AnswerMap()
        {
            Table("Answer");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.Description).Not.Nullable();
            Map(c => c.Vote).Default("0");
            Map(c=> c.IsAccepted).Not.Nullable();
            Map(c=> c.QuestionId).Not.Nullable();
            Map(c => c.UserId).Not.Nullable();
        }
    }
}
