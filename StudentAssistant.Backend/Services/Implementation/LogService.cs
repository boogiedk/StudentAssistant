using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Log;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        public async Task<LogDtoResponseModel> Get()
        {
            try
            {
                var fileName = Path.Combine("Storages", "Nlog",
                    " nlog-all-" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log");
                
                var resultDto = new LogDtoResponseModel();

                using (var streamReader = new StreamReader(fileName))
                {
                    var json = await streamReader.ReadToEndAsync();

                    resultDto.Logs = json;
                }

                return resultDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Get Exception: " + ex);
                throw new NotSupportedException();
            }
        }

        public async Task<LogDtoResponseModel> GetByType(string type)
        {
            try
            {
                var fileName = Path.Combine("Storages", "Nlog",
                    $" nlog-{type}-" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log");

                var resultDto = new LogDtoResponseModel();

                using (var streamReader = new StreamReader(fileName))
                {
                    var json = await streamReader.ReadToEndAsync();

                    resultDto.Logs = json;
                }

                return resultDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetByType Exception: " + ex);
                throw new NotSupportedException();
            }
        }
    }
}