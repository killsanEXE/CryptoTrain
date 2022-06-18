using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        readonly ITokenService _tokenService;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly IEmailService _emailService;
        public AccountController(ITokenService tokenService, UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IEmailService emailService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login()
        {
            await _emailService.SendEmail(new("killsan.exe@gmail.com", "Hello", "New app"));
            return NotFound();
        }
    }
}