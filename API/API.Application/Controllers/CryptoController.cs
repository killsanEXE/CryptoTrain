using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    [Authorize]
    public class CryptoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWrapper _wrapper;

        public CryptoController(IUnitOfWork unitOfWork, IWrapper wrapper)
        {
            _unitOfWork = unitOfWork;
            _wrapper = wrapper;
        }

        [HttpGet("btc")]
        public async Task<ActionResult<IEnumerable<BTC>>> GetBTC([FromQuery] UserParams userParams)
        {
            var btcs = await _unitOfWork.CryptoRepository.GetBitcoinHistoryAsync(userParams);
            return Ok(btcs);
        }
    }
}