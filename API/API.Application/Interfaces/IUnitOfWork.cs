using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Complete();
        IClientRepository ClientRepository { get; }
        bool HasChanges();
    }
}