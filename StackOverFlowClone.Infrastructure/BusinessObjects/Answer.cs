namespace StackOverFlowClone.Infrastructure.BusinessObjects
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Vote { get; set; }
        public bool IsAccepted { get; set; }
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
    }
}
