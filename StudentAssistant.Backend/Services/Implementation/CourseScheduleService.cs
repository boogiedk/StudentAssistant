using AutoMapper;
using Humanizer;
using Microsoft.Extensions.Options;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly ICourseScheduleDataService _courseScheduleDataService;
        private readonly IMapper _mapper;

        public CourseScheduleService(IParityOfTheWeekService parityOfTheWeekService, IMapper mapper, ICourseScheduleDataService courseScheduleDataService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
            _mapper = mapper;
            _courseScheduleDataService = courseScheduleDataService;
        }

        public List<CourseScheduleResultModel> GetCourseSchedule(CourseScheduleRequestModel input)
        {
            if (input == null) throw new NullReferenceException("Запрос не содержит данных.");

            var courseScheduleParameters = new CourseScheduleParameters
            {
                NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(input.DateTimeRequest),
                NameOfDayWeek = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(input.DateTimeRequest.DayOfWeek),
                ParityWeek = _parityOfTheWeekService.GetParityOfTheWeekByDateTime(input.DateTimeRequest)          
            };

            var courseScheduleDatabaseModel = _courseScheduleDataService.GetCourseScheduleFromDatabase(courseScheduleParameters);

            var courseScheduleModel = _mapper.Map<List<CourseScheduleResultModel>>(courseScheduleDatabaseModel);

            return courseScheduleModel;
        }

        public List<CourseScheduleViewModel> PrepareCourseScheduleViewModel(List<CourseScheduleResultModel> input)
        {
            var courseScheduleViewModel = _mapper.Map<List<CourseScheduleViewModel>>(input);

            var sortedcourseScheduleViewModel = courseScheduleViewModel.OrderBy(o => o.CourseNumber).ToList();

            return sortedcourseScheduleViewModel;
        }
    }
}
