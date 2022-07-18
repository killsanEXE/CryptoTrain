using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
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

        public async Task<IEnumerable<BTC>> GetBitcoinHistoryAsync()
        {
            return await _context.BTCs.ToListAsync();
        }
    }
}