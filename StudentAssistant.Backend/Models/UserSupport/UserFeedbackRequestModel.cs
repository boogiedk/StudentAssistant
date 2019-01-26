
namespace StudentAssistant.Backend.Models.UserSupport
{
    /// <summary>
    /// Модель для отправления отзыва пользователя.
    /// </summary>
    public class UserFeedbackRequestModel
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string EmailTo {get;set;}

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string TextBody { get; set; }
    }
}
