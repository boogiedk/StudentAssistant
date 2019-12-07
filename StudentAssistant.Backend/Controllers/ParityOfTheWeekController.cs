using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для получения данных о дне недели.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/parity")]
    [AllowAnonymous]
    [EnableCors("CorsPolicy")]
    public class ParityOfTheWeekController : Controller
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly ILogger<ParityOfTheWeekController> _logger;

        public ParityOfTheWeekController(
            IParityOfTheWeekService parityOfTheWeekService,
            ILogger<ParityOfTheWeekController> logger)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для получения данных о указанном дне недели.
        /// </summary>
        /// <returns><see cref="ParityOfTheWeekViewModel"/>Модель представления.</returns>
        /// <param name="requestModel">Модель, содержащая выбранную дату.</param>
        [HttpPost("selected")]
        public IActionResult GenerateParityOfTheWeek([FromBody] ParityOfTheWeekRequestModel requestModel)
        {
            try
            {
                if (requestModel == null)
                {
                    _logger.LogInformation("Request model is null");
                    return BadRequest("Запрос не содержит данных.");
                }

                _logger.LogInformation("Request: ", requestModel.SelectedDateTime);

                var userAccountRequestData = new UserAccountRequestDataParityOfTheWeek
                {
                    TimeZoneId = TimeZoneInfo.Local.Id //"Russian Standard Time"
                };

                var dateTimeOffsetRequestUtc = requestModel.SelectedDateTime;

                var dateTimeOffsetRequestUser = TimeZoneInfo.ConvertTime(dateTimeOffsetRequestUtc,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                // генерируем модель с данными о заданном дне.
                var parityOfTheWeekModel = _parityOfTheWeekService.GenerateDataOfTheWeek(dateTimeOffsetRequestUser);

                // подготавливаем модель для отображения (ViewModel)
                var resultViewModel = _parityOfTheWeekService.PrepareViewModel(parityOfTheWeekModel);

                _logger.LogInformation("Response: ", resultViewModel.ToString());
                return Ok(resultViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: ", ex);
                return BadRequest(ex);
            }
        }
    }
}