using System;
using Microsoft.AspNetCore.Mvc;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Services;

namespace StudentAssistant.Backend.Controllers
{

    [Produces("application/json")]
    [Route("api/schedule")]
    public class CourseScheduleController : ControllerBase
    {
        private readonly ICourseScheduleService _courseScheduleService;

        public CourseScheduleController(ICourseScheduleService courseScheduleService)
        {
            _courseScheduleService = courseScheduleService;
        }

        [HttpGet]
        [Route("today")]
        public IActionResult GetCourseScheduleToday()
        {
            try
            {
                  /*
                 * В будущем, когда будет разработан сервис авторизации и аутентификации, в качестве параметра в каждый метод контроллера будет передаваться
                 * модель с данными, в том числе, данные о часовом поясе. Соответственно, цепочка вызова метода сервиса будет такова:
                 * запрос клиента -> получение времени в UTC на сервере -> прибавление к времени UTC кол-во часов из пояса -> вызов метода сервиса.
                 */

                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = "Russian Standard Time"
                };

                var dateTimeOffsetRequestUtc = DateTimeOffset.UtcNow;

                var dateTimeOffsetRequestUser = TimeZoneInfo.ConvertTime(dateTimeOffsetRequestUtc,
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                var courseScheduleRequestModel = new CourseScheduleRequestModel
                {
                    DateTimeRequest = dateTimeOffsetRequestUser
                };

                // отправляем запрос на получение расписания
                var courseScheduleResultModel = _courseScheduleService.GetCourseSchedule(courseScheduleRequestModel);

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = _courseScheduleService.PrepareCourseScheduleViewModel(courseScheduleResultModel);

                return Ok(courseScheduleViewModel);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("tomorrow")]
        public IActionResult GetCourseScheduleTomorrow()
        {
            try
            {
                 /*
                 * В будущем, когда будет разработан сервис авторизации и аутентификации, в качестве параметра в каждый метод контроллера будет передаваться
                 * модель с данными, в том числе, данные о часовом поясе. Соответственно, цепочка вызова метода сервиса будет такова:
                 * запрос клиента -> получение времени в UTC на сервере -> прибавление к времени UTC кол-во часов из пояса -> вызов метода сервиса.
                 */

                var userAccountRequestData = new UserAccountRequestDataCourseSchedule
                {
                    TimeZoneId = "Russian Standard Time"
                };

                // берем utc время
                var dateTimeOffsetRequestUtc = DateTimeOffset.UtcNow;

                // переводим utc время в часовой пояс пользователя
                var dateTimeOffsetRequestUser = TimeZoneInfo.ConvertTime(dateTimeOffsetRequestUtc, 
                    TimeZoneInfo.FindSystemTimeZoneById(userAccountRequestData.TimeZoneId));

                // добавляем 1 день для отображения расписания
                dateTimeOffsetRequestUser = dateTimeOffsetRequestUser.AddDays(1);

                var courseScheduleRequestModel = new CourseScheduleRequestModel
                {
                    DateTimeRequest = dateTimeOffsetRequestUser
                };

                // отправляем запрос на получение расписания
                var courseScheduleResultModel = _courseScheduleService.GetCourseSchedule(courseScheduleRequestModel);

                // подготавливаем ViewModel для отображения
                var courseScheduleViewModel = _courseScheduleService.PrepareCourseScheduleViewModel(courseScheduleResultModel);

                return Ok(courseScheduleViewModel);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }
    }
}