using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;

namespace API.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<List<SingleTransactionDTO>> GetClientTransactionsAsync(string username);
        Task<AppUser> GetClientAsync(string username);
    }
}