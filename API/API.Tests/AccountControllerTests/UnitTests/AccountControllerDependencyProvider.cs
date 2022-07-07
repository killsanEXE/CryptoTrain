using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Controllers;
using Xunit.Abstractions;

namespace API.Tests.Tests.AccountControllerTests.UnitTests
{
    public class AccountControllerDependencyProvider : UnitTestDependencyProvider
    {
        protected readonly AccountController _accountController = null!;

        public AccountControllerDependencyProvider()
        {
            _accountController = new AccountController(_fakeTokenService.Object, 
            _fakeUserManager.Object, _fakeEmailService.Object, 
            _fakeSignInManager.Object, _mapper, _fakeWrapper.Object);
        }
    }
}