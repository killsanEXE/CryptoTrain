using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Controllers;

namespace API.Tests.Tests.AccountControllerTests
{
    public class AccountControllerDependencyProvider : DependencyProvider
    {
        protected readonly AccountController _accountController = null!;

        public AccountControllerDependencyProvider()
        {
            _accountController = new AccountController(_fakeTokenService, 
                _fakeUserManager, _fakeRoleManager, _fakeEmailService);
        }
    }
}