using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.DTOs;
using API.Application.Entities;
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
        readonly UserManager<AppUser> _userManager;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        public ClientController(IWrapper wrapper, UserManager<AppUser> userManager,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _wrapper = wrapper;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<TransactionsDTO>> GetTransactions()
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(f => f.UserName == _wrapper.GetUsernameViaWrapper(User));

            if(user == null) return NotFound("This user does not exists");
            
            return Ok(new TransactionsDTO()
            {
                Transactions = await _unitOfWork.ClientRepository
                    .GetClientTransactionsAsync(user.UserName)
            });
        }
    }
}