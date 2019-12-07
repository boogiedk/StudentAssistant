using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с расписанием.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/schedule")]
    [AllowAnonymous]
    [EnableCors("CorsPolicy")]
    public class CourseScheduleController : Controller
    {
        private readonly ICourseScheduleService _courseScheduleService;
        private readonly ILogger<CourseScheduleController> _logger;

        public CourseScheduleController(ICourseScheduleService courseScheduleService, ILogger<CourseScheduleController> logger)
        {
            _courseScheduleService = courseScheduleService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для получения расписания на выбранный день.
        /// </summary>
        /// <para name="requestModel">Модель запроса для получения расписания.</para>
        /// <returns><see cref="CourseScheduleViewModel"/> Модель представления расписания.</returns>
        [HttpPost("selected")]
        public IActionResult GetCourseScheduleSelected(
            [FromBody]CourseScheduleRequestModel requestModel)
        {
            try
            {
                _logger.Log(LogLevel.Information,"Request is here",requestModel.DateTimeRequest);

                // часовой пояс пользователя (по умолчанию - Москва, +3 часа к UTC)
                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = TimeZoneInfo.Local.Id //"Russian Standard Time"
                };

                // если модель пришла пустая, запрашиваем данные по текущему дню
                var datetimeUtc = requestModel?.DateTimeRequest ?? DateTime.UtcNow;

                // переводим utc время в часовой пояс пользователя
                var datetimeRequestUser = TimeZoneInfo.ConvertTime(datetimeUtc,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                var courseScheduleDtoModel = new CourseScheduleDtoModel
                {
                    DateTimeRequest = datetimeRequestUser,
                    GroupName = requestModel?.GroupName
                };

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = _courseScheduleService.Get(courseScheduleDtoModel);

                return Ok(courseScheduleViewModel);
            }
            catch (Exception)
            {
                return BadRequest(new CourseScheduleViewModel());
            }
        }

        /// <summary>
        /// Метод для получения расписания на сегодня.
        /// </summary>
        /// <returns><see cref="CourseScheduleViewModel"/> Модель представления расписания.</returns>
        [HttpGet("today")]
        public IActionResult GetToday()
        {
            try
            {
                // часовой пояс пользователя (по умолчанию - Москва, +3 часа к UTC)
                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = TimeZoneInfo.Local.Id, //"Russian Standard Time"
                    GroupName = "БББО-01-16"
                };

                var datetimeUtc = DateTime.UtcNow;

                // переводим utc время в часовой пояс пользователя
                var datetimeRequestUser = TimeZoneInfo.ConvertTime(datetimeUtc,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                var courseScheduleDtoModel = new CourseScheduleDtoModel
                {
                    DateTimeRequest = datetimeRequestUser,
                    GroupName = userAccountRequestData.GroupName
                };

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = _courseScheduleService.Get(courseScheduleDtoModel);

                return Ok(courseScheduleViewModel);
            }
            catch (Exception)
            {
                return BadRequest(new CourseScheduleViewModel());
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
                await _courseScheduleService.DownloadAsync(cancellationToken);

                return Ok("Данные обновлены!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        /// <summary>
        /// Метод для скачивания файла с расписанием по ссылке.
        /// </summary>
        /// <returns></returns>
        [HttpPost("download")]
        public async Task<IActionResult> DownloadCourseScheduleFileAsync(
            CourseScheduleUpdateByLinkAsyncModel request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _courseScheduleService.DownloadByLinkAsync(request, cancellationToken);

                return Ok("Данные обновлены!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        /// <summary>
        /// Метод для обновления расписания в базе данных.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("update")]
        public async Task<IActionResult> UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _courseScheduleService.UpdateAsync(cancellationToken);

                return Ok("Данные обновлены!");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Метод для получения даты последнего изменения файла с расписанием.
        /// </summary>
        /// <returns></returns>
        [HttpGet("lastupdate")]
        public async Task<IActionResult> GetLastUpdateCourseSchedule()
        {
            try
            {
                var result = await _courseScheduleService.GetLastAccessTimeUtc();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}