using AutoMapper;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly ICourseScheduleDataService _courseScheduleDataService;
        private readonly IImportDataExcelService _importDataExcelService;
        private readonly IMapper _mapper;

        public CourseScheduleService(IParityOfTheWeekService parityOfTheWeekService,
            IMapper mapper, ICourseScheduleDataService courseScheduleDataService,
            IImportDataExcelService importDataExcelService)
        {
            _parityOfTheWeekService = parityOfTheWeekService;
            _mapper = mapper;
            _courseScheduleDataService = courseScheduleDataService;
            _importDataExcelService = importDataExcelService;
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

                // отправляем запрос на получение расписания по указанным параметрам
                //   var courseScheduleDatabaseModel = _courseScheduleDataService.GetCourseScheduleFromJsonFile(courseScheduleParameters);

                var courseScheduleDatabaseModel = _courseScheduleDataService.GetCourseScheduleFromExcelFile(courseScheduleParameters);

                var courseScheduleModel = _mapper.Map<List<CourseScheduleResultModel>>(courseScheduleDatabaseModel);

                return courseScheduleModel;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }

        public CourseScheduleViewModel PrepareCourseScheduleViewModel(List<CourseScheduleResultModel> input)
        {
            if (input == null) throw new NullReferenceException("Запрос не содержит данных.");

            try
            {
                // если отсутствуют данные о расписании, возвращаем дефолтную модель
                // TODO: продумать более корректную дефолтную модель
                if (!input.Any())
                {
                    var emptyCourseScheduleViewModel = new CourseScheduleViewModel()
                    {
                        NameOfDayWeek = input.FirstOrDefault()?.NameOfDayWeek?.ToUpper(),
                        CoursesViewModel = new List<CoursesViewModel>() { new CoursesViewModel() {
                        CourseName = "Данных не найдено",
                        TeacherFullName = "Данных не найдено",
                        CourseType = "Данных не найдено",
                        CoursePlace = "Данных не найдено",
                        ParityWeek = "Данных не найдено" }
                        }
                    };

                    return emptyCourseScheduleViewModel;
                }

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
