using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/support")]
    public class UserSupportController : ControllerBase
    {
        private readonly IUserSupportService _userSupportService;
        private readonly IValidationService _validationService;

        public UserSupportController(IUserSupportService userSupportService, IValidationService validationService)
        {
            _userSupportService = userSupportService;
            _validationService = validationService;
        }

        [HttpPost]
        [Route("sendfeedback")]
        public IActionResult SendFeedback([FromBody]UserFeedbackRequestModel input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("Запрос не содержит данных.");
                }

                // проверяем корректность данных в модели.
                var resultValidationServiceModel = _validationService.ValidateRequest(input);

                // если модель содержит ошибки, которые определил сервис валидации, то
                // вернуть BadRequest()
                if(resultValidationServiceModel.Any())
                {
                    return BadRequest(resultValidationServiceModel);
                }

                // отправляем фидбек 
                var result = _userSupportService.SendFeedback(input);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }
    }
}