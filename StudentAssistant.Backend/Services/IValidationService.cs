using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.Validation;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Сервис для валидации входных запросов.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Метод для валидации входных запросов.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ValidationResultModel> ValidateRequest(IEnumerable<ModelError> input);

        /// <summary>
        /// Метод для валидации модели после отправки отзыва.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ValidationResultModel> PrepareErrorResult(UserFeedbackResultModel input);
    }
}
