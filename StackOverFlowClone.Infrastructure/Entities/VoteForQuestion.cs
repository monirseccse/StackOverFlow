using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Infrastructure.Entities
{
    public class VoteForQuestion : IEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual Guid QuestionId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual bool isUpVote { get; set; }
        public virtual bool isDownVote { get; set; }
    }
}