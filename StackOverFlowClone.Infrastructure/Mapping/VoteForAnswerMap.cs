using FluentNHibernate.Mapping;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Mapping
{
    public class VoteForAnswerMap : ClassMap<VoteForAnswer>
    {
        public VoteForAnswerMap()
        {
            Table("VoteForAnswer");

            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.UserId).Not.Nullable();
            Map(c => c.AnswerId).Not.Nullable();
            Map(c => c.isUpVote).Not.Nullable();
            Map(c => c.isDownVote).Not.Nullable();
        }
    }
}
