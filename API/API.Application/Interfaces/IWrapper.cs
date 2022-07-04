using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Interfaces
{
    public interface IWrapper
    {
        Task<bool> UserExistsAsync(UserManager<AppUser> userManager, string username);
        string url(HttpRequest request);
    }
}