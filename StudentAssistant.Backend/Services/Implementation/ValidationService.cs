using StudentAssistant.Backend.Models.UserSupport;
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

            if(input.Email==null)
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит E-mail." });

            if (input.TextBody == null)
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит текста." });

            if (input.Subject == null)
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит заголовка." });

            if (input.UserName == null)
                errorList.Add(new ValidationResultModel() { ErrorMessage = "Запрос не содержит имени пользователя." });


            return errorList;
        }
    }
}
