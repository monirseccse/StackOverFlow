using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Infrastructure.BusinessObjects
{
    public class VoteForQuestion
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public bool isUpVote { get; set; }
        public bool isDownVote { get; set; }
    }
}
