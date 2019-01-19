using StudentAssistant.Backend.Models.UserSupport;

namespace StudentAssistant.Backend.Services
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
    }
}
