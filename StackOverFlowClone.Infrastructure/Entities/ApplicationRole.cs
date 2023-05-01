

using FluentNHibernate.AspNetCore.Identity;

namespace StackOverFlowClone.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}
