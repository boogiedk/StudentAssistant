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
       public void Send_EmailResultModel_ShouldBeReturnErrorResult()
        {
            var emailService = new EmailService();

            var result = emailService.Send(new EmailRequestModel());
            
            Assert.False(result.IsSended);
        }
    }
}