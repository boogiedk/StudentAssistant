
namespace StudentAssistant.Backend.Models.Email
{
    /// <summary>
    /// Модель для отправки E-mail сообщения.
    /// </summary>
    public class EmailRequestModel
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Почта пользователя, куда адресовано сообщение.
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// Модель для хранения модели отправителя.
        /// </summary>
        public EmailAccountModel EmailAccount { get; set; }

    }
}