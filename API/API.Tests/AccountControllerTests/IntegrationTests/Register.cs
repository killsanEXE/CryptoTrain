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
        public async Task Return200()
        {
            RegisterDTO dto = new() 
            {
                Name = "Kirill",
                Surname = "Zhurov",
                Username = "Killsan",
                Email = "someGmail@gmail.com",
                Password = "pass"
            };

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("OK", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Return400()
        {
            RegisterDTO dto = new();

            var response = await _client.PostAsJsonAsync("api/account/register", dto);

            Assert.Equal("BadRequest", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task Return400_2()
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
        public async Task Return400_3()
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
    }
}