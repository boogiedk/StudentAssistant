﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Сценарий]_[Ожидаемое поведение]_[Результат]
    /// </summary>

    public class UserSupportTests
    {
        // fix test
        [Fact]
        public void SendFeedback_ReturnsModelWithResult_ShouldBeReturnsError()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var emailServiceConfigurationModel = fixture.Freeze<Mock<IOptions<EmailServiceConfigurationModel>>>();
            var userFeedbackRequestModel = new UserFeedbackRequestModel
            {
                EmailTo = "EmailTo",
                Subject = "Subject",
                TextBody = "TextBody",
                UserName = "UserName"
            };
            var emailAccountModel = new EmailAccountModel
            {
                EmailFrom = emailServiceConfigurationModel.Object.Value.EmailFrom,
                Id = emailServiceConfigurationModel.Object.Value.Id,
                HiddenEmail = emailServiceConfigurationModel.Object.Value.HiddenEmail,
                InputEnableSSL = emailServiceConfigurationModel.Object.Value.InputEnableSSL,
                InputHost = emailServiceConfigurationModel.Object.Value.InputHost,
                Login = emailServiceConfigurationModel.Object.Value.Login,
                OutputPort = emailServiceConfigurationModel.Object.Value.OutputPort,
                Password = emailServiceConfigurationModel.Object.Value.Password,
                UseDefaultCredentials = emailServiceConfigurationModel.Object.Value.UseDefaultCredentials,
                OutputEnableSSL = emailServiceConfigurationModel.Object.Value.OutputEnableSSL,
                OutputHost = emailServiceConfigurationModel.Object.Value.OutputHost,
                InputPort = emailServiceConfigurationModel.Object.Value.InputPort
            };
   
            var emailRequestModel = new EmailRequestModel
            {
                EmailTo = "EmailTo",
                EmailAccount = emailAccountModel
            };

            var userSupportService = fixture.Freeze<Mock<IUserSupportService>>();
            // new UserSupportService(mapper.Object, emailService.Object, emailServiceConfigurationModel.Object);
            var emailService = fixture.Freeze<Mock<IEmailService>>();

            emailService.Setup(s => s.SendEmail(emailRequestModel))
                .Returns(new EmailResultModel
            {
                IsSended = true,
                Message = "Сообщение успешно отправлено."
            });

            userSupportService.Setup(s => s.PrepareUserFeedbackRequestForEmailService(userFeedbackRequestModel))
                .Returns(emailRequestModel);

            fixture.Inject(emailService);
            fixture.Inject(userSupportService);

            // Act
            var resultService = fixture.Create<IUserSupportService>(); 
            var result = resultService.SendFeedback(userFeedbackRequestModel);


            // Assert
            Assert.AreEqual(false, result.IsSended);
        }
    }
}
