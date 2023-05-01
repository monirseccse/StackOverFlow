using StackOverFlowClone.Infrastructure.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Infrastructure.Services
{
    public interface IVoteForQuestionService
    {
        Task<VoteForQuestion> GetVoteDetail(Guid questionId, Guid userId);
        Task<VoteForQuestion> GetVoteDetailByQuestionId(Guid questionId);
        Task CreateVoteAsync(VoteForQuestion vote);
        Task DeleteVoteAsync(Guid id);
        Task EditVoteAsync(VoteForQuestion id);
    }
}
