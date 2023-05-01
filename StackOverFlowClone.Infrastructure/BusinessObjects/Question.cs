namespace StackOverFlowClone.Infrastructure.BusinessObjects
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TriedAndExpecting { get; set; }
        public string Tag { get; set; }
        public string DuplicateQuestion { get; set; }
        public int Vote { get; set; }
        public int Answer { get; set; }
        public Guid UserId { get; set; }
    }
}
