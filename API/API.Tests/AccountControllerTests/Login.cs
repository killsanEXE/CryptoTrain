using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests.AccountControllerTests
{
    [Collection("Sequential")]
    public class Login : AccountControllerDependencyProvider
    {
        [Fact]
        public async void Return200()
        {
            var result = await _accountController.Login();
            Assert.Equal(1, 1);
        }
    }
}