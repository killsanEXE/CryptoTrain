using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;
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

        public async Task<List<SingleTransactionDTO>> GetClientTransactionsAsync(string username)
        {
            AppUser? user = await _context.Users
                .Include(f => f.Transactions)
                .SingleOrDefaultAsync(f => f.UserName == username);
            if(user == null) return null!;

            return user.Transactions!.AsQueryable().AsNoTracking()
                .ProjectTo<SingleTransactionDTO>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public async Task<AppUser> GetClientAsync(string username)
        {
            return (await _context.Users.SingleOrDefaultAsync(f => f.UserName == username))!;
        }

    }
}