using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Controllers;
using API.Tests.Tests;

namespace API.Tests.ClientControllerTests.UnitTests
{
    public class ClientControllerDependencyProvider : UnitTestDependencyProvider
    {
        protected readonly ClientController _clientController = null!;

        public ClientControllerDependencyProvider()
        {
            _clientController = new ClientController(_fakeWrapper.Object, _mapper, 
                _fakeUnitOfWork.Object);
        }
    }
}