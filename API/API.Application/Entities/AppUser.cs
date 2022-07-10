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
        public string? Surname { get; set; }
        public ICollection<AppUserRole>? UserRoles { get; set; }
        public float USDAmount { get; set; }
        public float BTCAmount { get; set; }
        public DateTime LastReplenishmentDate { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}