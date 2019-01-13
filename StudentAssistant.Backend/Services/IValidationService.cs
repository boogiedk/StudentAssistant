using StudentAssistant.Backend.Models.UserSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services
{
    public interface IValidationService
    {
        List<ValidationResultModel> ValidateRequest(UserFeedbackRequestModel input);
    }
}
