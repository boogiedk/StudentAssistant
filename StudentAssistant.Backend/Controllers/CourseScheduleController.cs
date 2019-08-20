﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Services;
using StudentAssistant.DbLayer.Services;

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

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        /// <param name="courseScheduleService"></param>
        public CourseScheduleController(ICourseScheduleService courseScheduleService)
        {
            _courseScheduleService = courseScheduleService;
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
                // часовой пояс пользователя (по умолчанию - Москва, +3 часа к UTC)
                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = TimeZoneInfo.Local.Id //"Russian Standard Time"
                };

                // если модель пришла пустая, запрашиваем данные по текущему дню
                var dateTimeOffsetRequestUtc = requestModel?.DateTimeRequest ?? DateTimeOffset.Now;

                // переводим utc время в часовой пояс пользователя
                var dateTimeOffsetRequestUser = TimeZoneInfo.ConvertTime(dateTimeOffsetRequestUtc,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                var courseScheduleDtoModel = new CourseScheduleDtoModel
                {
                    DateTimeRequest = dateTimeOffsetRequestUser,
                    GroupName = requestModel?.GroupName
                };

                // отправляем запрос на получение расписания
                var courseScheduleResultModel = _courseScheduleService.Get(courseScheduleDtoModel);

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = _courseScheduleService.PrepareViewModel(courseScheduleResultModel, courseScheduleDtoModel.DateTimeRequest);

                return Ok(courseScheduleViewModel);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Метод для обновления данных о расписании в базе данных.
        /// </summary>
        /// <returns></returns>
        [HttpGet("update")]
        public async Task<IActionResult> UpdateAsyncCourseSchedule(
            CancellationToken cancellationToken)
        {
            try
            {
                await _courseScheduleService.UpdateAsync(cancellationToken);

                return Ok("Данные обновлены!");
            }
            catch (Exception ex)
            {
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
                var result = _courseScheduleService.GetLastAccessTimeUtc();

                return Ok(await result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}