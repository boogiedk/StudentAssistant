﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.DbLayer.Interfaces;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с расписанием.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/schedule")]
    [EnableCors("CorsPolicy")]
    public class CourseScheduleController : Controller
    {
        private readonly ICourseScheduleService _courseScheduleService;
        private readonly ILogger<CourseScheduleController> _logger;

        public CourseScheduleController(
            ICourseScheduleService courseScheduleService,
            ILogger<CourseScheduleController> logger)
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
        public async Task<IActionResult> GetCourseScheduleSelected(
            [FromBody] CourseScheduleRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Request: " + requestModel);

                // часовой пояс пользователя (по умолчанию - Москва, +3 часа к UTC)
                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = TimeZoneInfo.Local.Id //"Russian Standard Time"
                };

                // переводим utc время в часовой пояс пользователя
                var datetimeRequestUser = TimeZoneInfo.ConvertTime(requestModel.DateTimeRequest,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                var courseScheduleDtoModel = new CourseScheduleDtoModel
                {
                    DateTimeRequest = datetimeRequestUser,
                    GroupName = requestModel.GroupName
                };

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = await _courseScheduleService.Get(courseScheduleDtoModel);

                _logger.LogInformation("Response: " + courseScheduleViewModel);
                return Ok(courseScheduleViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
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

                _logger.LogInformation("Request: ",
                    "DateTimeRequest: " + datetimeUtc, " ",
                    "GroupName " + userAccountRequestData.GroupName);

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

                _logger.LogInformation("Response: " + courseScheduleViewModel);
                return Ok(courseScheduleViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: " + ex);
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
                var response = await _courseScheduleService.DownloadAsync(cancellationToken);

                _logger.LogInformation($"Response: " + "Данные обновлены!");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: " + ex);
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
                _logger.LogInformation("Request: " + request.Uri);
                await _courseScheduleService.DownloadByLinkAsync(request, cancellationToken);

                _logger.LogInformation("Response: " + "Данные обновлены!");
                return Ok("Данные обновлены!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: " + ex);
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
                var response = await _courseScheduleService.UpdateAsync(cancellationToken);

                _logger.LogInformation("Response: " + "Данные обновлены!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
                return BadRequest(ex);
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

                _logger.LogInformation("Response: " + result.UpdateDatetime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Метод для добавления расписания в базу данных. (временно)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("insert")]
        public async Task<IActionResult> InsertAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _courseScheduleService.InsertAsync(cancellationToken);

                _logger.LogInformation("Response: " + "Данные вставлены!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Метод для пометки как удаленное в базе данных. (временно)
        /// </summary>
        /// <returns></returns>
        [HttpGet("marklikedeleted")]
        public IActionResult MarkLikeDeleted()
        {
            try
            {
                _courseScheduleService.MarkLikeDeleted();

                _logger.LogInformation("Response: " + "Данные помечены!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex);
                return BadRequest(ex);
            }
        }
    }
}