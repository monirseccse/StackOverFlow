namespace StackOverFlowClone.Infrastructure.Entities
{
    public class Answer : IEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual int Vote { get; set; }
        public virtual bool IsAccepted { get; set; }
        public virtual Guid QuestionId { get; set; }
        public virtual Guid UserId { get; set; }
    }
}
