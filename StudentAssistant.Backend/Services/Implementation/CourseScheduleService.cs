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

            try
            {
                // подготавливаем параметры для получения расписания
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
            catch(Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }

        public CourseScheduleViewModel PrepareCourseScheduleViewModel(List<CourseScheduleResultModel> input)
        {
            if (input == null) throw new NullReferenceException("Запрос не содержит данных.");

            try
            {
                // маппим список предметов из бд в модель представления
                var coursesViewModel = _mapper.Map<List<CoursesViewModel>>(input);

                // сортируем по позиции в раписании
                var sortedCoursesViewModel = coursesViewModel.OrderBy(o => o.CourseNumber).ToList();

                // создаем результирующую модель представления
                var resultCourseScheduleViewModel = new CourseScheduleViewModel
                {
                    CoursesViewModel = sortedCoursesViewModel,
                    NameOfDayWeek = input.FirstOrDefault()?.NameOfDayWeek?.ToUpper()
                };

                return resultCourseScheduleViewModel;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }
    }
}
