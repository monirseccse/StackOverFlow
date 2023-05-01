using NHibernate;
using StackOverFlowClone.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public class VoteForQuestionRepository : Repository<VoteForQuestion, Guid>, IVoteForQuestionRepository
    {
        public VoteForQuestionRepository(ISession session) : base(session)
        {
        }
    }
}
