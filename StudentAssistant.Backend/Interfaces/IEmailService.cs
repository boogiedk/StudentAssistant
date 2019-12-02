using StudentAssistant.Backend.Models.Email;

namespace StudentAssistant.Backend.Interfaces
{
    /// <summary>
    /// E-mail сервис.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Отправляет E-mail сообщение
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EmailResultModel Send(EmailRequestModel input);
    }
}
