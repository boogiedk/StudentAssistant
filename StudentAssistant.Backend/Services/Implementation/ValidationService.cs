﻿using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Models.Validation;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudentAssistant.Backend.Interfaces;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ValidationService : IValidationService
    {
        public List<ValidationResultModel> ValidateRequest(IEnumerable<ModelError> input)
        {
            List<ValidationResultModel> errorList = new List<ValidationResultModel>();

            if (input == null)
                errorList.Add(new ValidationResultModel { ErrorMessage = "Запрос не содержит данных." });

            foreach(var errorModel in input)
            {
                errorList.Add(new ValidationResultModel { ErrorMessage = errorModel.ErrorMessage });
            }

            return errorList;
        }

        public ValidationResultModel PrepareErrorResult(UserFeedbackResultModel input)
        {
            ValidationResultModel errorModel;

            if (input == null)
            {
                errorModel = new ValidationResultModel { ErrorMessage = "Запрос не содержит данных." };

                return errorModel;
            }

            errorModel = new ValidationResultModel { ErrorMessage = input.Message };

            return errorModel;
        }
    }
}
