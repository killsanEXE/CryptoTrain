using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Data
{
    public class ClientRepository : IClientRepository
    {
        readonly ApplicationContext _context;
        readonly IMapper _mapper;
        public ClientRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUser> GetClientAsync(string username)
        {
            return (await _context.Users.SingleOrDefaultAsync(f => f.UserName == username))!;
        }

        public async Task<PagedList<SingleTransactionDTO>> GetClientTransactionsAsync(PaginationParams paginationParams, string username)
        {
            var query = _context.Transactions.Where(f => f.User!.UserName == username)
                .AsQueryable();
            query = query.OrderBy(f => f.TransactionDate).Reverse();

            return await PagedList<SingleTransactionDTO>.CreateAsync(
                query.ProjectTo<SingleTransactionDTO>(_mapper.ConfigurationProvider).AsNoTracking(),
                paginationParams.PageNumber, paginationParams.PageSize
            );
        }
    }
}