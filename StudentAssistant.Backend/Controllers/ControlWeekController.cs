using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ControlWeek;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с расписанием.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/controlWeek")]
    [EnableCors("CorsPolicy")]
    [AllowAnonymous]
    public class ControlWeekController : Controller
    {
        private readonly IControlWeekService _controlWeekService;
        private readonly ILogger<CourseScheduleController> _logger;

        public ControlWeekController(
            IControlWeekService controlWeekService,
            ILogger<CourseScheduleController> logger)
        {
            _controlWeekService = controlWeekService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для получения расписания зачетной недели.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetControlWeek(ControlWeekRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Request: " + requestModel);
                

                _logger.LogInformation("Response: " + requestModel);
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