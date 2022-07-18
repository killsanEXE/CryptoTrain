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
using System.Security.Claims;

namespace API.Tests.ClientControllerTests.UnitTests
{
    public class ReplenishUSDTests : ClientControllerDependencyProvider
    {
        [Fact]
        public async Task User_Is_Null_Returns_NotFound()
        {
            AppUser? user = null!;
            string username = "username";

            _fakeWrapper.Setup(f => f.GetUsernameViaWrapper(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);
            _fakeUnitOfWork.Setup(f => f.ClientRepository.GetClientAsync(username))
                .ReturnsAsync(user);

            var actionResult = await _clientController.ReplenishUSD();

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result?.StatusCode);
        }

        [Fact]
        public async Task Less_Than_30_Days_Since_Last_Replenish_Returns_BadRequest()
        {
            AppUser user = new()
            {
                LastReplenishmentDate = DateTime.Today,
                USDAmount = 2000
            };
            string username = "username";

            _fakeWrapper.Setup(f => f.GetUsernameViaWrapper(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);
            _fakeUnitOfWork.Setup(f => f.ClientRepository.GetClientAsync(username))
                .ReturnsAsync(user);

            var actionResult = await _clientController.ReplenishUSD();

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task Failed_To_Save_Changes_In_Db_Returns_BadRequest()
        {
            AppUser user = new()
            {
                LastReplenishmentDate = DateTime.Today.AddDays(-31),
                USDAmount = 2000
            };
            string username = "username";

            _fakeWrapper.Setup(f => f.GetUsernameViaWrapper(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);
            _fakeUnitOfWork.Setup(f => f.ClientRepository.GetClientAsync(username))
                .ReturnsAsync(user);
            _fakeUnitOfWork.Setup(f => f.Complete()).ReturnsAsync(false);

            var actionResult = await _clientController.ReplenishUSD();

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task Successfull_Replensih_Returns_OkResult()
        {
            AppUser user = new()
            {
                LastReplenishmentDate = DateTime.Today.AddDays(-31),
                USDAmount = 2000
            };
            string username = "username";

            _fakeWrapper.Setup(f => f.GetUsernameViaWrapper(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);
            _fakeUnitOfWork.Setup(f => f.ClientRepository.GetClientAsync(username))
                .ReturnsAsync(user);
            _fakeUnitOfWork.Setup(f => f.Complete()).ReturnsAsync(true);

            var actionResult = await _clientController.ReplenishUSD();

            var result = actionResult.Result as OkObjectResult;
            var resultDTO = result?.Value as UserDTO;
            Assert.Equal(200, result?.StatusCode);
            Assert.Equal(user.USDAmount, resultDTO?.USDAmount);
        }
    }
}