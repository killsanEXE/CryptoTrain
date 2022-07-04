using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Entities;
using API.Application.Helpers;
using API.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit.Abstractions;

namespace API.Tests.Tests
{
    public class DependencyProvider
    {
        protected static IMapper _mapper = null!;

        protected readonly Mock<ITokenService> _fakeTokenService;
        protected readonly Mock<IEmailService> _fakeEmailService;
        protected readonly Mock<UserManager<AppUser>> _fakeUserManager;
        protected readonly Mock<RoleManager<AppRole>> _fakeRoleManager;
        protected readonly Mock<SignInManager<AppUser>> _fakeSignInManager;
        protected readonly Mock<IWrapper> _fakeWrapper;

        public DependencyProvider()
        {
            var mapingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            _mapper = mapingConfig.CreateMapper();

            _fakeTokenService = new Mock<ITokenService>();
            _fakeEmailService = new Mock<IEmailService>();


            var fakeUserStore = new Mock<IUserStore<AppUser>>();           
            _fakeUserManager = new Mock<UserManager<AppUser>>(fakeUserStore.Object, null, 
                null, null, null, null, null, null, null);
            var fakeRoleStore = new Mock<IRoleStore<AppRole>>();
            _fakeRoleManager = new Mock<RoleManager<AppRole>>(fakeRoleStore.Object, null, 
                null, null, null);
            _fakeSignInManager = new Mock<SignInManager<AppUser>>(_fakeUserManager.Object, 
                Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);

            _fakeWrapper = new Mock<IWrapper>();
        }
    }
}