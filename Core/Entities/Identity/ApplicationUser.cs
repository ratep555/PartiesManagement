using System.Collections.Generic;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}