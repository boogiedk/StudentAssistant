using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.LogProvider;

namespace StudentAssistant.Backend.Controllers
{
    /// <summary>
    /// Контроллер с методами для работы с логами.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/log")]
    [EnableCors("CorsPolicy")]
    [AllowAnonymous]
    public class LogController : Controller
    {
        private readonly ILogger<LogController> _logger;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogController(
            ILogService logService,
            IMapper mapper, ILogger<LogController> logger)
        {
            _logService = logService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var logDtoModel = await _logService.Get();

                var logResponseModel = _mapper.Map<LogResponseModel>(logDtoModel);
                
                _logger.LogInformation("Response: response is " + string.IsNullOrEmpty(logResponseModel.Logs));

                return Ok(logResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Request Exception: " + ex);
                return BadRequest();
            }
        }

        [HttpPost("Getbytype")]
        public async Task<IActionResult> GetByType(LogRequestModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("В запросе отсутствуют данные.");
                }
                
                _logger.LogInformation("Request: " + request.LogType);

                var logDtoModel = await _logService.GetByType(request.LogType);

                var logResponseModel = _mapper.Map<LogResponseModel>(logDtoModel);
                
                _logger.LogInformation("Response: response is " + string.IsNullOrEmpty(logResponseModel.Logs));

                return Ok(logResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Request Exception: " + ex);
                return BadRequest();
            }
        }
    }
}