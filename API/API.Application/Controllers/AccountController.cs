using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Controllers
{
    public class AccountController : BaseController
    {
        readonly ITokenService _tokenService;
        readonly UserManager<AppUser> _userManager;
        readonly IEmailService _emailService;
        readonly SignInManager<AppUser> _signInManager;
        readonly IMapper _mapper;
        readonly IWrapper _wrapper;

        readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        public AccountController(ITokenService tokenService, UserManager<AppUser> userManager,
            IEmailService emailService, SignInManager<AppUser> signInManager, 
            IMapper mapper, IWrapper wrapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _mapper = mapper;
            _wrapper = wrapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            AppUser? user = await _userManager.FindByNameAsync(loginDTO.Username);
            if(user == null) return Unauthorized("Invalid username");

            if(!user.EmailConfirmed) return BadRequest("Email was not confirmed");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if(!result.Succeeded) return Unauthorized("Invalid password");

            return Ok(new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            registerDTO = TrimStrings<RegisterDTO>(registerDTO);
            if(await _wrapper.UserExistsAsync(_userManager, registerDTO.Username!)) 
                return BadRequest("User already exists");
            
            var user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username;

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            var role = await _userManager.AddToRoleAsync(user, "Client");
            if(!role.Succeeded) return BadRequest(role.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string message = $"{_wrapper.url(this.Request)}/api/account/confirm-email/{user.UserName}/{token}";
            await _emailService.SendEmail(
                new EmailMessage(registerDTO.Email!, "Confirm email", message)
            );
            return Ok();
        }

        [HttpGet("confirm-email/{username}/{**token}")]
        public async Task<ActionResult> ConfirmEmail(string username, string token)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null || user.EmailConfirmed) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded) return Content("Email was confirmed");
            return Content("Failed to confirm email");
        }

        [HttpPost("forgot-password/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) return BadRequest("User with this email does not exists");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            string url;
            if(env == "Development") url = "https://localhost:4200";
            else if(env == "Docker") url = "http://localhost:4200";
            else url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            string message = $"{url}/reset-password?token={token}";
            await _emailService.SendEmail(new EmailMessage(user.Email!, "Click the link to reset your password", message));
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null) return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, dto.Token!.Replace(" ", "+"), dto.Password);
            if(result.Succeeded) return new EmptyResult();
            return BadRequest(result.Errors.FirstOrDefault()?.Description ?? "Failed to reset password");
        }

        [HttpPost("resend-email-confirmation/{email}")]
        public async Task<ActionResult> ResendEmailConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null) return BadRequest("User with this email does not exists");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string message = $"{_wrapper.url(this.Request)}/api/account/confirm-email/{user.UserName}/{token}";
            await _emailService.SendEmail(new EmailMessage(email, "Confirm email", message));
            return Ok();
        }

        T TrimStrings<T>(T obj) where T: class
        {
            var stringProperties = obj.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof (string));
            foreach (var stringProperty in stringProperties)
            {
                if(stringProperty.Name != "Password"){
                    string currentValue = (string) stringProperty.GetValue(obj, null)!;
                    stringProperty.SetValue(obj, currentValue.Trim(), null);
                }
            }
            return obj;
        }
    }
}