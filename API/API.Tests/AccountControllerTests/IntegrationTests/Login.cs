using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace API.Tests.AccountControllerTests.IntegrationTests
{
    [Collection("Sequential")]
    public class Login : IntegrationTestDependencyProvider
    {
        [Fact]
        public async Task Return401()
        {
            LoginDTO dto = new()
            {
                Username = "leon",
                Password = "leonsPassword"
            };

            var response = await _client.PostAsJsonAsync("api/account/login", dto);

            Assert.Equal("Unauthorized", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Return400()
        {
            LoginDTO dto = new()
            {
                Username = "fam",
                Password = "pass"
            };

            var response = await _client.PostAsJsonAsync("api/account/login", dto);

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Return401_1()
        {
            LoginDTO dto = new()
            {
                Username = "lisa",
                Password = "password"
            };

            var response = await _client.PostAsJsonAsync("api/account/login", dto);

            Assert.Equal("Unauthorized", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Return200()
        {
            LoginDTO dto = new()
            {
                Username = "lisa",
                Password = "pass"
            };

            var response = await _client.PostAsJsonAsync("api/account/login", dto);

            var userDTO = await response.Content.ReadFromJsonAsync<UserDTO>();
            Assert.Equal("OK", response?.StatusCode.ToString());
            Assert.Equal(dto.Username, userDTO?.Username);
            Assert.NotNull(userDTO?.Token);
        }
    }
}