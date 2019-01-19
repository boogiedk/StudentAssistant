namespace StudentAssistant.Backend.Models.Email
{
    /// <summary>
    /// Модель, возвращающая результат запроса на отправку сообщения.
    /// </summary>
    public class EmailResultModel
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