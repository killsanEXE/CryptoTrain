using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace API.Tests.ClientControllerTests.IntegrationTests
{
    [Collection("Sequential")]
    public class ReplenishUSDTests : IntegrationTestDependencyProvider
    {
        [Fact]
        public async Task User_Is_Not_Authorized_Return_Unauthorized()
        {
            var response = await _client.PutAsJsonAsync("api/client/replenish-usd", new {});

            Assert.Equal("Unauthorized", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Less_Than_30_Days_Since_Last_Replenish_Returns_BadRequest()
        {
            await AuthenticateAsync("killsan");

            var response = await _client.PutAsJsonAsync("api/client/replenish-usd", new {});

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Successfull_Replensih_Returns_OkResult()
        {
            await AuthenticateAsync("lisa");

            var response = await _client.PutAsJsonAsync("api/client/replenish-usd", new {});

            var result = await response.Content.ReadFromJsonAsync<UserDTO>();
            Assert.Equal(10000, result?.USDAmount);
            Assert.Equal("OK", response?.StatusCode.ToString());
        }
    }
}