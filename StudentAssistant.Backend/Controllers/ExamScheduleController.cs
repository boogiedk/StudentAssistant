using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ExamSchedule;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с расписанием экзаменационной сессии.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/examSchedule")]
    [EnableCors("CorsPolicy")]
    [AllowAnonymous]
    public class ExamScheduleController : Controller
    {
        private readonly IExamScheduleService _examScheduleService;
        private readonly ILogger<ExamScheduleController> _logger;

        public ExamScheduleController(
            IExamScheduleService examScheduleService,
            ILogger<ExamScheduleController> logger)
        {
            _examScheduleService = examScheduleService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для получения расписания на экзаменационную сессию.
        /// </summary>
        /// <para name="requestModel">Модель запроса для получения расписания.</para>
        /// <returns><see cref="ExamScheduleRequestModel"/> Модель представления расписания.</returns>
        [HttpPost("Get")]
        public async Task<IActionResult> GetExamSchedule([FromBody] ExamScheduleRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Request: " + requestModel);

                var result = await _examScheduleService.Get(requestModel);

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
        /// Метод для скачивания файла с расписанием экзаменов.
        /// </summary>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<IActionResult> DownloadExamScheduleFileAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _examScheduleService.DownloadAsync(cancellationToken);
                _logger.LogInformation($"Response: " + "Данные обновлены!");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: " + ex);
                return BadRequest(ex);
            }
        }
    }
}