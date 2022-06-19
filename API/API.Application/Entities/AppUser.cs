using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public ICollection<AppUserRole>? UserRoles { get; set; }
    }
}