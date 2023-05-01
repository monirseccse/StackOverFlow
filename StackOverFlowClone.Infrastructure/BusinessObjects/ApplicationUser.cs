using Microsoft.AspNetCore.Identity;

namespace StackOverFlowClone.Infrastructure.BusinessObjects
{
    public class ApplicationUser : IdentityUser<Guid>
    {       
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
