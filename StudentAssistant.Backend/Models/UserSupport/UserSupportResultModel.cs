namespace StudentAssistant.Backend.Models.UserSupport
{
    /// <summary>
    ///  Модель, возвращающая результат запроса на отправку сообщения для поддержки.
    /// </summary>
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