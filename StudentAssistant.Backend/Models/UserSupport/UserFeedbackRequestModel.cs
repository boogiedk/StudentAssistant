using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [StringLength(50, ErrorMessage = "Превышен размер длины имени пользователя")]
        public string UserName { get; set; }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        [Required(ErrorMessage = "Не указан почтовый адрес")]
        [EmailAddress(ErrorMessage = "Некорректный почтовый адрес")]
        [StringLength(50,ErrorMessage = "Превышен размер длины почтового адреса")]
        public string EmailTo {get;set;}

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        [Required(ErrorMessage = "Не указана тема сообщения")]
        [StringLength(50,ErrorMessage = "Превышен размер длины темы сообщения")]
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [Required(ErrorMessage = "Не указан текст сообщения")]
        [StringLength(500, ErrorMessage = "Превышен размер длины текста сообщения")]
        public string TextBody { get; set; }
    }
}
