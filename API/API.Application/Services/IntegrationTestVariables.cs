using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Interfaces;

namespace API.Application.Services
{
    public class IntegrationTestVariables : IIntegrationTestVariables
    {
        public bool CurrentlyTesting()
        {
            return false;
        }
    }
}