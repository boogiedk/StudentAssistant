namespace StudentAssistant.Backend.Models.DownloadAsync
{
    /// <summary>
    /// Модель с ответом об обновленном расписании.
    /// </summary>
    public class DownloadAsyncResponseModel
    {
        /// <summary>
        /// Флаг, новый ли файл.
        /// </summary>
        public bool IsNewFile { get; set; }
        /// <summary>
        /// Ответ для пользователя.
        /// </summary>
        public string Message { get; set; }
    }
}