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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        readonly IWrapper _wrapper;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;

        public ClientController(IWrapper wrapper, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _wrapper = wrapper;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<SingleTransactionDTO>>> GetTransactions(
            [FromQuery] PaginationParams paginationParams)
        {
            var transactions = await _unitOfWork.ClientRepository
                .GetClientTransactionsAsync(paginationParams, _wrapper
                .GetUsernameViaWrapper(User));

            _wrapper.AddPaginationHeaderViaWrapper(Response, transactions.CurrentPage, 
                transactions.PageSize, transactions.TotalCount, transactions.TotalPages);
            return Ok(transactions);
        }

        [HttpPut("replenish-usd")]
        public async Task<ActionResult<UserDTO>> ReplenishUSD()
        {
            var user = await _unitOfWork.ClientRepository
                .GetClientAsync(_wrapper.GetUsernameViaWrapper(User));
            if(user == null) return NotFound("This user does not exists");

            var today = DateTime.Today; 
            var daysSinceLastReplenishment = (today - user.LastReplenishmentDate).TotalDays;
            if(daysSinceLastReplenishment < 30)
            {
                return BadRequest($"You can replenish your bank account in {30-daysSinceLastReplenishment} day(s)");
            }
            else
            {
                user.USDAmount = user.USDAmount + 5000;
                user.LastReplenishmentDate = today;
                if(await _unitOfWork.Complete())
                    return new UserDTO() { USDAmount = user.USDAmount };
                else return BadRequest("Could not replenish your bank accound");
            }
        }
    }
}