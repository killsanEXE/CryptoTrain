using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Data
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CryptoRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<BTC>> GetBitcoinHistoryAsync(UserParams userParams)
        {
            var query = _context.BTCs.AsNoTracking().AsQueryable().OrderBy(f => f.Id);
            return await PagedList<BTC>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public void AddNewBTC(BTC btc)
        {
            _context.BTCs.Add(btc);
        }
    }
}