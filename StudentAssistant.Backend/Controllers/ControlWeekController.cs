using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с расписанием зачётной недели.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/controlWeek")]
    [EnableCors("CorsPolicy")]
    [AllowAnonymous]
    public class ControlWeekController : Controller
    {
        private readonly IControlWeekService _controlWeekService;
        private readonly ILogger<ControlWeekController> _logger;

        public ControlWeekController(
            IControlWeekService controlWeekService,
            ILogger<ControlWeekController> logger)
        {
            _controlWeekService = controlWeekService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для получения расписания на зачетную сессию.
        /// </summary>
        /// <para name="requestModel">Модель запроса для получения расписания.</para>
        /// <returns><see cref="ControlWeekViewModel"/> Модель представления расписания.</returns>
        [HttpPost("Get")]
        public async Task<IActionResult> GetControlWeek([FromBody] ControlWeekRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Request: " + requestModel);

                var result = await _controlWeekService.Get(requestModel);

                _logger.LogInformation("Response: " + requestModel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Метод для скачивания файла с расписанием.
        /// </summary>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<IActionResult> DownloadCourseScheduleFileAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                await _controlWeekService.DownloadAsync(cancellationToken);

                _logger.LogInformation($"Response: " + "Данные обновлены!");
                return Ok("Данные обновлены!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: " + ex);
                return BadRequest(ex);
            }
        }
    }
}