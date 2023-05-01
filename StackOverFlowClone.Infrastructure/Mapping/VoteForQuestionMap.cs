using FluentNHibernate.Mapping;
using StackOverFlowClone.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Infrastructure.Mapping
{
    public class VoteForQuestionMap : ClassMap<VoteForQuestion>
    {
        public VoteForQuestionMap()
        {
            Table("VoteForQuestion");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.UserId).Not.Nullable();
            Map(c => c.QuestionId).Not.Nullable();
            Map(c => c.isUpVote).Not.Nullable();
            Map(c => c.isDownVote).Not.Nullable();
        }
    }
}
