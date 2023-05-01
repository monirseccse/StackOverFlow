namespace StackOverFlowClone.Infrastructure.Entities
{
    public class Question : IEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string TriedAndExpecting { get; set; }
        public virtual string Tag { get; set; }
        public virtual string DuplicateQuestion { get; set; }
        public virtual int Vote { get; set; }
        public virtual int Answer { get; set; }
        public virtual Guid UserId { get; set; }
    }
}
