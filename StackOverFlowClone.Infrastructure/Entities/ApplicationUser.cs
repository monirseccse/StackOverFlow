using FluentNHibernate.AspNetCore.Identity;

namespace StackOverFlowClone.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {       
        public virtual string Name { get; set; }
    }
}
