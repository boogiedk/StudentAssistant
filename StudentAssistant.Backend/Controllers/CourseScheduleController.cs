using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                var dateTimeNow = DateTime.Now;

               // dateTimeNow = new DateTime(2019, 02, 20); тест

                var courseScheduleRequestModel = new CourseScheduleRequestModel
                {
                    DateTimeRequest = dateTimeNow
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
                var dateTimeNow = DateTime.Now;

                // добавляем +1 день
                dateTimeNow = dateTimeNow.AddDays(1); 

                var courseScheduleRequestModel = new CourseScheduleRequestModel
                {
                    DateTimeRequest = dateTimeNow
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