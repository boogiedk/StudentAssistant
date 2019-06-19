using AutoMapper;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleDatabaseService _courseScheduleDatabaseService;
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IDownloadExcelFileService _downloadExcelFileService;
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly IMapper _mapper;

        public CourseScheduleService(
            IMapper mapper, ICourseScheduleFileService courseScheduleFileService,
            ICourseScheduleDatabaseService courseScheduleDatabaseService,
            IDownloadExcelFileService downloadExcelFileService,
            IParityOfTheWeekService parityOfTheWeekService
            )
        {
            _courseScheduleDatabaseService = courseScheduleDatabaseService ?? throw new ArgumentNullException(nameof(courseScheduleDatabaseService));
            _courseScheduleFileService = courseScheduleFileService ?? throw new ArgumentNullException(nameof(courseScheduleFileService));
            _downloadExcelFileService = downloadExcelFileService ?? throw new ArgumentNullException(nameof(downloadExcelFileService));
            _parityOfTheWeekService = parityOfTheWeekService ?? throw new ArgumentNullException(nameof(parityOfTheWeekService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<CourseScheduleResultModel> Get(CourseScheduleDtoModel input)
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
                //   var courseScheduleDatabaseModel = _courseScheduleFileService.GetCourseScheduleFromJsonFileByParameters(courseScheduleParameters);

                // на данным момент расписание берется из Excel файла.
                var courseScheduleDatabaseModel = _courseScheduleFileService
                    .GetCourseScheduleFromExcelFileByParameters(courseScheduleParameters);

                var courseScheduleModel = _mapper.Map<List<CourseScheduleResultModel>>(courseScheduleDatabaseModel);

                return courseScheduleModel;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }

        public CourseScheduleViewModel PrepareViewModel(List<CourseScheduleResultModel> input)
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
                        CoursesViewModel = new List<CourseViewModel>() { new CourseViewModel() {
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
                var coursesViewModel = _mapper.Map<List<CourseViewModel>>(input);

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

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // проверяем свежесть файла
                var isNewFile = _downloadExcelFileService.CheckCurrentExcelFile(DateTimeOffset.UtcNow);

                // если не свежий => качаем новый (не менее часа)
                if (!isNewFile.Result) await _downloadExcelFileService.DownloadAsync(cancellationToken);

                // берем лист из excel файла
                var courseScheduleDatabaseModels = _courseScheduleFileService.GetFromExcel();

                // отправляем запрос на сохранения данных в бд
                await _courseScheduleDatabaseService.UpdateAsync(courseScheduleDatabaseModels);

            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
        }
    }
}
