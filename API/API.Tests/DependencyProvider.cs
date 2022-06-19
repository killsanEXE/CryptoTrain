using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace API.Tests.Tests
{
    public class DependencyProvider
    {
        protected static ITokenService _fakeTokenService = null!;
        protected static IMapper _mapper = null!;
        protected static IEmailService _fakeEmailService = null!;
        protected static UserManager<AppUser> _fakeUserManager = null!;
        protected static RoleManager<AppRole> _fakeRoleManager = null!;
        public DependencyProvider()
        {
            var mapingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            _mapper = mapingConfig.CreateMapper();

            _fakeTokenService = A.Fake<ITokenService>();
            _fakeEmailService = A.Fake<IEmailService>();


            var fakeUserStore = A.Fake<IUserStore<AppUser>>();
            _fakeUserManager = A.Fake<UserManager<AppUser>>(f => 
                f.WithArgumentsForConstructor(() => new UserManager<AppUser>
                (fakeUserStore, null, null, null, null, null, null, null, null)));
            _fakeUserManager.UserValidators.Add(new UserValidator<AppUser>());
            _fakeUserManager.PasswordValidators.Add(new PasswordValidator<AppUser>());

            _fakeRoleManager = A.Fake<RoleManager<AppRole>>();
        }
    }
}