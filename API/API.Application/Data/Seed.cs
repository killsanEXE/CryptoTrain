using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Helpers
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;

            List<AppUser> clients = new()
            {
                new() 
                { 
                    UserName = "eliot", 
                    Name="Eliot", 
                    Surname="Anderson", 
                    Email = "eliot123@gmail.com", 
                    EmailConfirmed = true
                },
                new() 
                { 
                    UserName = "lisa", 
                    Name="Lisa", 
                    Surname="Money", 
                    Email = "lisa123@gmail.com", 
                    EmailConfirmed = true
                },
                new() 
                { 
                    UserName = "tom", 
                    Name="Tom", 
                    Surname="Hardy", 
                    Email = "tom123@gmail.com", 
                    EmailConfirmed = true
                },
                new()
                {
                    UserName = "fam",
                    Name = "Mate",
                    Surname = "",
                    Email = "goMakeMeSandwich@gmail.com",
                    EmailConfirmed = false
                }
            };

            var roles = new List<AppRole>
            {
                new() { Name = "Client"},
                new() { Name = "Admin"}
            };

            foreach(var role in roles) await roleManager.CreateAsync(role);

            foreach(var user in clients)
            {
                await userManager.CreateAsync(user, "pass");
                await userManager.AddToRoleAsync(user, "Client");
            }

            var admin = new AppUser
            {
                UserName = "admin",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Perehod2020");
            await userManager.AddToRolesAsync(admin, new[] {"Admin"});
        }
    }
}