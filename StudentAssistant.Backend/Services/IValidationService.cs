using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.Validation;
using System.Collections.Generic;

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
        List<ValidationResultModel> ValidateRequest(UserFeedbackRequestModel input);
    }
}
