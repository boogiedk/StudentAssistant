using System;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class JwtTokenFactoryTests
    {
        [Theory]
        [InlineData("861123FC-241D-4E19-96E2-ABB8FB46C621", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        [InlineData("5AB47F7A-79BD-4E8F-B650-52BDF2D8A803", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        [InlineData("D874298C-1741-4E5E-9C5C-89224CB1ED79", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        public async void CreateJwtToken_ReturnsJwtToken_ShouldBeReturnCorrectToken(string input, string expectedValue)
        {
            var jwtFactory = new JwtTokenFactory();

            var token = await jwtFactory.CreateJwtToken(input);

            Assert.Contains(expectedValue, token);
        }

        [Theory]
        [InlineData("")]
        public void CreateJwtToken_ReturnsException_ShouldBeReturnException(string input)
        {
            var jwtFactory = new JwtTokenFactory();

            Assert.ThrowsAsync<ArgumentException>(async () => await jwtFactory.CreateJwtToken(input));
        }
    }
}