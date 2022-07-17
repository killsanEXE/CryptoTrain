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
    public class GetTransactionsTests : IntegrationTestDependencyProvider
    {
        [Fact]
        public async Task User_Is_Not_Authorized_Return_Unauthorized()
        {
            var response = await _client.GetAsync("api/client/transactions?pageNumber=1&pageSize=5");

            Assert.Equal("Unauthorized", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task User_Is_Authorized_Return_OkObjectResult()
        {
            await AuthenticateAsync("killsan");
            var response = await _client.GetAsync("api/client/transactions?pageNumber=1&pageSize=5");

            var result = await response.Content.ReadFromJsonAsync<SingleTransactionDTO[]>();
            Assert.Equal(5, result?.Length);
            Assert.Equal("OK", response?.StatusCode.ToString());
        }

        [Fact]
        public async Task User_Authorized_With_Different_Pagination_Params()
        {
            await AuthenticateAsync("killsan");
            var response = await _client.GetAsync("api/client/transactions?pageNumber=1&pageSize=10");

            var result = await response.Content.ReadFromJsonAsync<SingleTransactionDTO[]>();
            Assert.Equal(10, result?.Length);
            Assert.Equal("OK", response?.StatusCode.ToString());
        }
    }
}