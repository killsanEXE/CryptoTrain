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
    public class Register : IntegrationTestDependencyProvider
    {
        [Fact]
        public async Task Invalid_DTO_Returns_BadRequest()
        {
            RegisterDTO dto = new();

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task User_Already_Exists_Returns_BadRequest()
        {
            RegisterDTO dto = new() 
            {
                Name = "Kirill",
                Surname = "Zhurov",
                Username = "lisa",
                Email = "someGmail@gmail.com",
                Password = "pass"
            };

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Password_Does_Not_Match_Requirements_Return_BadRequest()
        {
            RegisterDTO dto = new() 
            {
                Name = "Kirill",
                Surname = "Zhurov",
                Username = "somenewUser123",
                Email = "someGmail@gmail.com",
                Password = "PASSWORD+031211212121"
            };

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Successfully_Creates_New_User_Returns_OkResult()
        {
            RegisterDTO dto = new() 
            {
                Name = "Kirill",
                Surname = "Zhurov",
                Username = "Killsan123",
                Email = "someGmail@gmail.com",
                Password = "pass"
            };

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("OK", response?.StatusCode.ToString());
        }
    }
}