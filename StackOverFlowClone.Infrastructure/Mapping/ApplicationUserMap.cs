using FluentNHibernate.AspNetCore.Identity.Mappings;
using StackOverFlowClone.Infrastructure.Entities;

namespace StackOverFlowClone.Infrastructure.Mapping
{
    public class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, Guid>
    {
        public ApplicationUserMap() : base(t => t.GeneratedBy.Guid()) // Primary key config
        {
            Map(x => x.Name).Not.Nullable();
        }
    }
}
