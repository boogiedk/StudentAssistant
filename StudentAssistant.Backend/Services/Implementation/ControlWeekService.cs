using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ControlWeekService : IControlWeekService
    {
        private readonly ICourseScheduleMongoDbService _courseScheduleMongoDbService;
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly ILogger<CourseScheduleService> _logger;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ControlWeekService(
            ICourseScheduleMongoDbService courseScheduleMongoDbService,
            ICourseScheduleFileService courseScheduleFileService,
            IParityOfTheWeekService parityOfTheWeekService,
            ILogger<CourseScheduleService> logger,
            IFileService fileService,
            IMapper mapper)
        {
            _courseScheduleMongoDbService = courseScheduleMongoDbService ??
                                            throw new ArgumentNullException(nameof(courseScheduleMongoDbService));
            _courseScheduleFileService = courseScheduleFileService ??
                                         throw new ArgumentNullException(nameof(courseScheduleFileService));
            _parityOfTheWeekService =
                parityOfTheWeekService ?? throw new ArgumentNullException(nameof(parityOfTheWeekService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        
         public async Task<ControlWeekViewModel> GetControlWeek(ControlWeekRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("GetControlWeek: " + $"{requestModel?.GroupName}");

                var controlWeekList = await _courseScheduleFileService.GetFromExcelFile();

                var controlWeekControlModel = await PrepareViewModel(controlWeekList);

                return controlWeekControlModel;

            }
            catch (Exception ex)
            {
                _logger.LogError("GetControlWeek Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

         private Task<ControlWeekViewModel> PrepareViewModel(List<CourseScheduleDatabaseModel> controlWeekList)
         {
             throw new NotImplementedException();
         }
    }
}