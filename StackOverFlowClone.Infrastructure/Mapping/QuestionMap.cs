using FluentNHibernate.Mapping;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Mapping
{
    public class QuestionMap : ClassMap<Question>
    {
        public QuestionMap()
        {
            Table("Question");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.Title).Not.Nullable();
            Map(c => c.Tag).Not.Nullable();
            Map(c => c.Description).Not.Nullable();
            Map(c => c.TriedAndExpecting).Not.Nullable();
            Map(c => c.DuplicateQuestion).Default("");
            Map(c => c.Vote).Default("0");
            Map(c => c.Answer).Default("0");
            Map(c => c.UserId).Not.Nullable();
        }
    }
}
