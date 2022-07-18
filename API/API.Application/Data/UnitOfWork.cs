using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Interfaces;
using AutoMapper;

namespace API.Application.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationContext _context;
        readonly IMapper _mapper;
        public UnitOfWork(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IClientRepository ClientRepository => new ClientRepository(_context, _mapper);

        public ICryptoRepository CryptoRepository => new CryptoRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}