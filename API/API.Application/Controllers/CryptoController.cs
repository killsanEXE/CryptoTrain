using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    [Authorize]
    public class CryptoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CryptoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("btc")]
        public async Task<ActionResult<IEnumerable<BTC>>> GetBTC()
        {
            var btcs = await _unitOfWork.CryptoRepository.GetBitcoinHistoryAsync();
            return Ok(btcs);
        }
    }
}