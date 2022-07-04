using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using Xunit;
using API.Application.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Tests.Tests.AccountControllerTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Identity;
using API.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Tests.AccountControllerTests
{
    [Collection("Sequential")]
    public class Register : AccountControllerDependencyProvider
    {

        readonly RegisterDTO dto = new() 
        {
            Username = "killsan",
            Name = "Kirill",
            Surname = "Zhurov",
            Email = "killsan.exe@gmail.com",
            Password = "pass"
        };

        [Fact]
        public async void Return400()
        {

            _fakeWrapper.Setup(f => f.UserExistsAsync(_fakeUserManager.Object, dto.Username!))
                .ReturnsAsync(true);

            var actionResult = await _accountController.Register(dto);

            var result = actionResult as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async void Return400_2()
        {

            List<IdentityError> errors = new()
            {
                new() { Code="Error1", Description="Ivalid something"},
                new() { Code="Error2", Description="Ivalid something"},
                new() { Code="Error3", Description="Ivalid something"}
            };
            Microsoft.AspNetCore.Identity.IdentityResult userManagerResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Failed(errors.ToArray());

            _fakeWrapper.Setup(f => f.UserExistsAsync(_fakeUserManager.Object, dto.Username!))
                .ReturnsAsync(false);
            _fakeUserManager.Setup(f => f.CreateAsync(It.IsAny<AppUser>(), dto.Password))
                .ReturnsAsync(userManagerResult);

            var actionResult = await _accountController.Register(dto);

            var result = actionResult as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async void Return400_3()
        {

            List<IdentityError> errors = new()
            {
                new() { Code="Error1", Description="Ivalid something"},
                new() { Code="Error2", Description="Ivalid something"},
                new() { Code="Error3", Description="Ivalid something"}
            };
            Microsoft.AspNetCore.Identity.IdentityResult userManagerResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Success;
            Microsoft.AspNetCore.Identity.IdentityResult userManagerRoleResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Failed(errors.ToArray());

            _fakeWrapper.Setup(f => f.UserExistsAsync(_fakeUserManager.Object, dto.Username!))
                .ReturnsAsync(false);
            _fakeUserManager.Setup(f => f.CreateAsync(It.IsAny<AppUser>(), dto.Password))
                .ReturnsAsync(userManagerResult);
            _fakeUserManager.Setup(f => f.AddToRoleAsync(It.IsAny<AppUser>(), "Client"))
                .ReturnsAsync(userManagerRoleResult);

            var actionResult = await _accountController.Register(dto);

            var result = actionResult as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async void Return200()
        {
            Microsoft.AspNetCore.Identity.IdentityResult userManagerResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Success;
            Microsoft.AspNetCore.Identity.IdentityResult userManagerRoleResult = 
                Microsoft.AspNetCore.Identity.IdentityResult.Success;

            _fakeWrapper.Setup(f => f.UserExistsAsync(_fakeUserManager.Object, dto.Username!))
                .ReturnsAsync(false);
            _fakeUserManager.Setup(f => f.CreateAsync(It.IsAny<AppUser>(), dto.Password))
                .ReturnsAsync(userManagerResult);
            _fakeUserManager.Setup(f => f.AddToRoleAsync(It.IsAny<AppUser>(), "Client"))
                .ReturnsAsync(userManagerRoleResult);
            _fakeWrapper.Setup(f => f.url(It.IsAny<HttpRequest>()))
                .Returns("url");

            var actionResult = await _accountController.Register(dto);

            _fakeUserManager.Verify(f => f.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()),
                Times.Once());
            _fakeEmailService.Verify(f => f.SendEmail(It.IsAny<EmailMessage>()),
                Times.Once());

            var result = actionResult as OkResult;
            Assert.Equal(200, result?.StatusCode);
        }
    }
}