using StudentAssistant.Backend.Models.UserSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services
{
    public interface IUserSupportService
    {
        /// <summary>
        /// Отправляет отзыв пользователя.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        UserFeedbackResultModel SendFeedback(UserFeedbackRequestModel input); 
    }
}
