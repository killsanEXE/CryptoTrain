using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using Xunit;
using API.Application.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Tests.Tests.AccountControllerTests.UnitTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Identity;
using API.Application.Helpers;

namespace API.Tests.AccountControllerTests
{
    [Collection("Sequential")]
    public class ConfirmEmail : AccountControllerDependencyProvider
    {
        string token = "token";

        [Fact]
        public async Task Return400()
        {
            AppUser? user = null!;

            _fakeUserManager.Setup(f => f.FindByNameAsync("username"))
                .ReturnsAsync(user);

            var actionResult = await _accountController.ConfirmEmail("username", token);

            var result = actionResult as NotFoundResult;
            Assert.Equal(404, result?.StatusCode);
        }

        [Fact]
        public async Task Return400_2()
        {
            AppUser user = new() { UserName = "username", EmailConfirmed = true };

            _fakeUserManager.Setup(f => f.FindByNameAsync("username"))
                .ReturnsAsync(user);

            var actionResult = await _accountController.ConfirmEmail("username", token);

            var result = actionResult as NotFoundResult;
            Assert.Equal(404, result?.StatusCode);
        }

        [Fact]
        public async Task ReturnContent_Invalid()
        {
            AppUser user = new() { UserName = "username", EmailConfirmed = false };
            Microsoft.AspNetCore.Identity.IdentityResult userManagerResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Failed();

            _fakeUserManager.Setup(f => f.FindByNameAsync("username"))
                .ReturnsAsync(user);
            _fakeUserManager.Setup(f => f.ConfirmEmailAsync(user, token))
                .ReturnsAsync(userManagerResult);

            var actionResult = await _accountController.ConfirmEmail("username", token);

            var result = actionResult as ContentResult;
            Assert.Equal("Failed to confirm email", result?.Content);
        }

        [Fact]
        public async Task ReturnContent_Valid()
        {
            AppUser user = new() { UserName = "username", EmailConfirmed = false };
            Microsoft.AspNetCore.Identity.IdentityResult userManagerResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Success;

            _fakeUserManager.Setup(f => f.FindByNameAsync("username"))
                .ReturnsAsync(user);
            _fakeUserManager.Setup(f => f.ConfirmEmailAsync(user, token))
                .ReturnsAsync(userManagerResult);

            var actionResult = await _accountController.ConfirmEmail("username", token);

            var result = actionResult as ContentResult;
            Assert.Equal("Email was confirmed", result?.Content);
        }
    }
}