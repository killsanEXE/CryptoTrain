using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Helpers
{
    public class Wrapper : IWrapper
    {
        public string url(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }

        public async Task<bool> UserExistsAsync(UserManager<AppUser> userManager, string username)
        {
            return await userManager.Users.AnyAsync(f => f.UserName == username.ToLower());
        }

    }
}