using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using StudentAssistant.Backend.Controllers;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.DbLayer;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.Tests.StudentAssistant.UnitTests.FakeClasses;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly IMapper _mapper;

        private readonly Mock<SignInManager<IdentityUser>> _signInManager;
        private readonly Mock<IJwtTokenFactory> _jwtTokenFactory;
        private readonly FakeSignInManager _fakeSignInManager;

        public AccountControllerTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _mockMapper = fixture.Freeze<Mock<IMapper>>();

            _jwtTokenFactory = new Mock<IJwtTokenFactory>();
            _signInManager = new Mock<SignInManager<IdentityUser>>();
            _fakeSignInManager = new FakeSignInManager();
        }

        [Fact]
        public void Register_ReturnToken_ShouldBeRegisterUser()
        {
        }
    }
}