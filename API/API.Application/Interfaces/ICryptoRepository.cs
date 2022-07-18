using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;

namespace API.Application.Interfaces
{
    public interface ICryptoRepository
    {
        Task<BTC[]> GetBitcoinHistoryAsync();
    }
}