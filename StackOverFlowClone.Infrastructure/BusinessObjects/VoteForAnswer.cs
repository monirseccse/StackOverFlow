
namespace StackOverFlowClone.Infrastructure.BusinessObjects
{
    public class VoteForAnswer
    {
        public  Guid Id { get; set; }
        public  Guid AnswerId { get; set; }
        public  Guid UserId { get; set; }
        public  bool isUpVote { get; set; }
        public  bool isDownVote { get; set; }
    }
}
