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

                // валидируем и в случае наличия ошибок подготавливаем модель для ответа клиенту
                if (!ModelState.IsValid)
                {
                    var modelsErrors = ModelState.Values.SelectMany(v => v.Errors);
                    var errorMessages = _validationService.ValidateRequest(modelsErrors);

                    // отправляем код 400 и результат валидации
                    return BadRequest(errorMessages);
                }

                // отправляем фидбек 
                var resultSendFeedback = _userSupportService.SendFeedback(input);

                // если сообщение не отправлено, подготавливаем ответ с текстом ошибки
                if (!resultSendFeedback.IsSended)
                {
                    var errorMessages = _validationService.PrepareErrorResult(resultSendFeedback);

                    return BadRequest(errorMessages);
                }

                // отправляем код 200 и результат отправки сообщения
                return Ok(resultSendFeedback);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }
    }
}