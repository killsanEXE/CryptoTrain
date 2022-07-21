using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Helpers;

namespace API.Application.Interfaces
{
    public interface ICryptoRepository
    {
        Task<PagedList<BTC>> GetBitcoinHistoryAsync(UserParams userParams);
        void AddNewBTC(BTC btc);
    }
}