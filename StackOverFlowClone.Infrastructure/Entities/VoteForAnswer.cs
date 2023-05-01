namespace StackOverFlowClone.Infrastructure.Entities
{
    public class VoteForAnswer : IEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AnswerId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual bool isUpVote { get; set; }
        public virtual bool isDownVote { get; set; }
    }
}
