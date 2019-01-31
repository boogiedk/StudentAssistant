using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ValidationService : IValidationService
    {
        public List<ValidationResultModel> ValidateRequest(IEnumerable<ModelError> input)
        {
            List<ValidationResultModel> errorList = new List<ValidationResultModel>();

            if (input == null)
                errorList.Add(new ValidationResultModel { ErrorMessage = "Запрос не содержит данных." });

            // создать общую модель ошибки для ответа клиенту

            return errorList;
        }

        public List<ValidationResultModel> PrepareErrorResult(UserFeedbackResultModel input)
        {
            List<ValidationResultModel> errorList = new List<ValidationResultModel>();

            if (input == null)
                errorList.Add(new ValidationResultModel { ErrorMessage = "Запрос не содержит данных." });

            // создать общую модель ошибки для ответа клиенту

            return errorList;
        }
    }
}
