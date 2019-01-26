using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ValidationService : IValidationService
    {
        public List<ValidationResultModel> ValidateRequest(UserFeedbackRequestModel input)
        {
            List<ValidationResultModel> errorList = new List<ValidationResultModel>();

            if (input == null)
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит данных." });

            //TODO:
            //проверить, что быстрее, equals, nullOrEmpty или input==null
            if (string.IsNullOrEmpty(input.EmailTo))
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит E-mail." });

            if (string.IsNullOrEmpty(input.TextBody))
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит текста." });

            if (string.IsNullOrEmpty(input.Subject))
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит заголовка." });

            if (string.IsNullOrEmpty(input.UserName))
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит имени пользователя." });


            return errorList;
        }
    }
}
