namespace StudentAssistant.Backend.Models.UserSupport
{
    public class UserSupportResultModel
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