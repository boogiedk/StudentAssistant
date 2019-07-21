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
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleDatabaseService _courseScheduleDatabaseService;
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly IDownloadFileService _downloadFileService;
        private readonly IMapper _mapper;

        public CourseScheduleService(
            ICourseScheduleFileService courseScheduleFileService,
            ICourseScheduleDatabaseService courseScheduleDatabaseService,
            IParityOfTheWeekService parityOfTheWeekService,
            IDownloadFileService downloadFileService,
            IMapper mapper
        )
        {
            _courseScheduleDatabaseService = courseScheduleDatabaseService ?? throw new ArgumentNullException(nameof(courseScheduleDatabaseService));
            _courseScheduleFileService = courseScheduleFileService ?? throw new ArgumentNullException(nameof(courseScheduleFileService));
            _parityOfTheWeekService = parityOfTheWeekService ?? throw new ArgumentNullException(nameof(parityOfTheWeekService));
            _downloadFileService = downloadFileService ?? throw new ArgumentNullException(nameof(downloadFileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<CourseScheduleResultModel> Get(CourseScheduleDtoModel input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

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
                throw new NotSupportedException("Ошибка во время выполнения.\n" + ex);
            }
        }

        public CourseScheduleViewModel PrepareViewModel(List<CourseScheduleResultModel> input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

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
                throw new NotSupportedException("Ошибка во время выполнения.\n" + ex);
            }
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // проверяем свежесть файла
                var isNewFile = _downloadFileService.CheckCurrentExcelFile(DateTimeOffset.UtcNow);

                // TODO: вынести в конфиг
                var downloadFileParametersModel = new DownloadFileParametersModel
                {
                    PathToFile = @"Infrastructure\ScheduleFile",
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/3d4/"),
                    FileNameLocal = "scheduleFile",
                    FileNameRemote = "KBiSP-3-kurs-2-sem",
                    FileFormat = "xlsx"
                };

                // если не свежий => качаем новый (1 сутки)
                if (!isNewFile.Result) await _downloadFileService.DownloadAsync(
                    downloadFileParametersModel, cancellationToken);

                // берем лист из excel файла
                var courseScheduleDatabaseModels = _courseScheduleFileService.GetFromExcel();

                // отправляем запрос на сохранения данных в бд (возможно, такого функционала и не появится)
                //  await _courseScheduleDatabaseService.UpdateAsync(courseScheduleDatabaseModels, cancellationToken);

            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. \n" + ex);
            }
        }
    }
}
