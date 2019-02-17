
namespace StudentAssistant.Backend.Models.UserSupport
{
    /// <summary>
    /// Модель, возвращающая результат запроса на отправку фидбека.
    /// </summary>
    public class UserFeedbackResultModel
    {
        /// <summary>
        /// Статус успешности запроса на отправку E-mail сообщения.
        /// </summary>
        public bool IsSended { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; set; }
    }
}
