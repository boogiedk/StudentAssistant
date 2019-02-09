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

                var courseScheduleRequestModel = new CourseScheduleRequestModel
                {
                    DateTimeRequest = dateTimeNow
                };

                var courseScheduleResultModel = _courseScheduleService.GetCourseSchedule(courseScheduleRequestModel);

                var courseScheduleViewModel = _courseScheduleService.PrepareCourseScheduleViewModel(courseScheduleResultModel);

                return Ok(courseScheduleResultModel);
            }
            catch (Exception ex)
            {
                // log
                return BadRequest(ex);
            }
        }
    }
}