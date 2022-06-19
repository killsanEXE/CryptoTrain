using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Helpers;

namespace API.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
    }
}