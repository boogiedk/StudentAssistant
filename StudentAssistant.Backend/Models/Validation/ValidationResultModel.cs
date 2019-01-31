using Microsoft.AspNetCore.Http;

namespace StudentAssistant.Backend.Models.Validation
{
    /// <summary>
    /// Модель с результатами валидации.
    /// </summary>
    public class ValidationResultModel
    {
        /// <summary>
        /// Текст с ошибкой.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}