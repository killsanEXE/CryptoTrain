using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;
using API.Application.Helpers;

namespace API.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<PagedList<SingleTransactionDTO>> GetClientTransactionsAsync(PaginationParams paginationParams, string username);
        Task<AppUser> GetClientAsync(string username);
    }
}