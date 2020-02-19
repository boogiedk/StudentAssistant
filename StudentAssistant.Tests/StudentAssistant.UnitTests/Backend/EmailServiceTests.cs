using System;
using AutoFixture;
using Moq;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class EmailServiceTests
    {
        [Fact]
        public void Send_EmailResultModel_ShouldBeReturnResultWithIsSendedFalse()
        {
            // Arrange
            var emailService = new EmailService();
            var requestModel = new EmailRequestModel
            {
                EmailAccount = new EmailAccountModel
                {
                    HiddenEmail = "test@email.com",
                    EmailFrom = "test@email.com",
                    InputPort = 80,
                    OutputHost = "test@email.com",
                    Password = "test@email.com",
                    UseDefaultCredentials = 1,
                    InputEnableSSL = 1,
                    OutputPort = 80
                },
                EmailTo = "test@email.com",
                TextBody = "Body",
                Subject = "Subject",
                UserName = "userName"
            };

            // Act
            var result = emailService.Send(requestModel);

            // Assert
            Assert.False(result.IsSended);
        }
        
        [Fact]
        public void Send_EmailResultModel_ShouldBeReturnResultWithIsSendedFalseAndMessageEmptyParameters()
        {
            // Arrange
            var emailService = new EmailService();
            var requestModel = new EmailRequestModel();

            // Act
            var result = emailService.Send(requestModel);

            // Assert
            Assert.False(result.IsSended);
        }

        [Fact]
        public void Send_EmailResultModel_ShouldBeReturnModelFromCatch()
        {
            // Arrange
            var emailService = new EmailService();
            var requestModel = new EmailRequestModel
                {EmailAccount = new EmailAccountModel()};

            // Act
            var result = emailService.Send(requestModel);

            // Assert
            Assert.False(result.IsSended);
            Assert.Equal(@"Произошла ошибка при отправке сообщения: возникло исключение в системе.", result.Message);
        }

        [Fact]
        public void Send_EmailResultModel_ShouldBeReturnResultWithIsSendedTrue()
        {
            // Arange
            var mock = new Mock<IEmailService>();
            var requestModel = new EmailRequestModel
            {
                EmailAccount = new EmailAccountModel
                {
                    HiddenEmail = "test@email.com"
                }
            };

            mock.Setup(a => a.Send(requestModel)).Returns(new EmailResultModel {IsSended = true});
            
            // Act
            var result = mock.Object.Send(requestModel);

            // Assert
            Assert.True(result.IsSended);
        }
    }
}