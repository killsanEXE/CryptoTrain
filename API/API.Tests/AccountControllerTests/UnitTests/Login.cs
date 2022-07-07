using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using Xunit;
using API.Application.Entities;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace API.Tests.Tests.AccountControllerTests.UnitTests
{
    [Collection("Sequential")]
    public class Login : AccountControllerDependencyProvider
    {
        [Fact]
        public async Task Return401()
        {
            LoginDTO dto = new() { Username = "Username", Password = "Password" };
            AppUser? user = null!;

            _fakeUserManager.Setup(f => f.FindByNameAsync(dto.Username))
                .ReturnsAsync(user);
            
            var actionResult = await _accountController.Login(dto);

            var result = actionResult.Result as UnauthorizedObjectResult;
            Assert.Equal(401, result?.StatusCode);
        }

        [Fact]
        public async Task Return400()
        {
            LoginDTO dto = new() { Username = "Username", Password = "Password" };
            AppUser? user = new() { UserName = dto.Username, EmailConfirmed = false };
            
            _fakeUserManager.Setup(f => f.FindByNameAsync(dto.Username))
                .ReturnsAsync(user);

            var actionResult = await _accountController.Login(dto);

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task Return401_2()
        {
            LoginDTO dto = new() { Username = "Username", Password = "Password" };
            AppUser? user = new() { UserName = dto.Username, EmailConfirmed = true };

            Microsoft.AspNetCore.Identity.SignInResult signInResult = 
                Microsoft.AspNetCore.Identity.SignInResult.Failed;

            _fakeUserManager.Setup(f => f.FindByNameAsync(dto.Username))
                .ReturnsAsync(user);
            _fakeSignInManager.Setup(f => f.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(signInResult);

            var actionResult = await _accountController.Login(dto);

            var result = actionResult.Result as UnauthorizedObjectResult;
            Assert.Equal(401, result?.StatusCode);
        }

        [Fact]
        public async Task Return200()
        {
            LoginDTO dto = new() { Username = "Username", Password = "Password" };
            AppUser? user = new() { UserName = dto.Username, EmailConfirmed = true };

            Microsoft.AspNetCore.Identity.SignInResult signInResult = 
                Microsoft.AspNetCore.Identity.SignInResult.Success;

            _fakeUserManager.Setup(f => f.FindByNameAsync(dto.Username))
                .ReturnsAsync(user);
            _fakeSignInManager.Setup(f => f.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(signInResult);
            _fakeTokenService.Setup(f => f.CreateTokenAsync(user))
                .ReturnsAsync("token");

            var actionResult = await _accountController.Login(dto);

            var result = actionResult.Result as OkObjectResult;
            var resultDTO = result?.Value as UserDTO;
            Assert.Equal(200, result?.StatusCode);
            Assert.Equal(dto.Username, resultDTO?.Username);
        }
    }
}