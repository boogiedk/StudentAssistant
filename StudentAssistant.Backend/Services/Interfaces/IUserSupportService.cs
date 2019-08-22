using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Models.UserSupport;

namespace StudentAssistant.Backend.Services.Interfaces
{
    /// <summary>
    /// Сервис для поддержки пользователей.
    /// </summary>
    public interface IUserSupportService
    {
        /// <summary>
        /// Отправляет отзыв пользователя.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        UserFeedbackResultModel SendFeedback(UserFeedbackRequestModel input);

        /// <summary>
        /// Метод для подготовки и маппинга модели <see cref="UserFeedbackRequestModel"/> в <see cref="EmailRequestModel"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EmailRequestModel PrepareUserFeedbackRequestForEmailService(UserFeedbackRequestModel input);
    }
}
