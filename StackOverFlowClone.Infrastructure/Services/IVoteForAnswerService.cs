using StackOverFlowClone.Infrastructure.BusinessObjects;

namespace StackOverFlowClone.Infrastructure.Services
{
    public interface IVoteForAnswerService
    {
        Task<VoteForAnswer> GetVoteDetail(Guid answerId, Guid userId);
        Task<VoteForAnswer> GetVoteDetailByAnswerId(Guid answerId);
        Task CreateVoteAsync(VoteForAnswer vote);
        Task DeleteVoteAsync(Guid id);
        Task EditVoteAsync(VoteForAnswer id);
    }
}
