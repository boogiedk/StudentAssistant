using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.UserSupport;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с обратной связью.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/support")]
    [Authorize]
    public class UserSupportController : Controller
    {
        private readonly IUserSupportService _userSupportService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        /// <param name="userSupportService"></param>
        /// <param name="validationService"></param>
        public UserSupportController(
            IUserSupportService userSupportService, 
            IValidationService validationService)
        {
            _userSupportService = userSupportService;
            _validationService = validationService;
        }

        /// <summary>
        /// Метод для отправки обратной связи.
        /// </summary>
        /// <param name="userFeedbackRequestModel">Модель, содержащая данные для отправки отзыва пользователя.</param>
        /// <returns></returns>
        [HttpPost("sendfeedback")]
        public IActionResult SendFeedback([FromBody]UserFeedbackRequestModel userFeedbackRequestModel)
        {
            try
            {
                if (userFeedbackRequestModel == null)
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
                var resultSendFeedback = _userSupportService.SendFeedback(userFeedbackRequestModel);

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