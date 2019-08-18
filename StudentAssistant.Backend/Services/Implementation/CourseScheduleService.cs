using AutoMapper;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Internal;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CourseScheduleService(
            ICourseScheduleFileService courseScheduleFileService,
            IParityOfTheWeekService parityOfTheWeekService,
            IFileService fileService,
            IMapper mapper
        )
        {
            _courseScheduleFileService = courseScheduleFileService ?? throw new ArgumentNullException(nameof(courseScheduleFileService));
            _parityOfTheWeekService = parityOfTheWeekService ?? throw new ArgumentNullException(nameof(parityOfTheWeekService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
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
                    NameOfDayWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(input.DateTimeRequest.DayOfWeek),
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
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public CourseScheduleViewModel PrepareViewModel(
            List<CourseScheduleResultModel> input, DateTimeOffset dateTimeRequest)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            try
            {
                // если отсутствуют данные о расписании, возвращаем пустую модель
                if (!input.Any())
                {
                    var emptyCourseScheduleViewModel = new CourseScheduleViewModel
                    {
                        NameOfDayWeek = input.FirstOrDefault()?.NameOfDayWeek?.ToUpper(),
                        DatetimeRequest = dateTimeRequest.Date.ToShortDateString(),
                        UpdateDatetime = _fileService.GetLastWriteTime().Result.ToShortDateString(),
                        CoursesViewModel = new List<CourseViewModel> { new CourseViewModel() }
                    };

                    return emptyCourseScheduleViewModel;
                }

                // маппим список предметов из бд в модель представления
                var coursesViewModel = _mapper.Map<List<CourseViewModel>>(input);

                // удаляем пустые предметы и сортируем по позиции в раписании
                var sortedCoursesViewModel = coursesViewModel
                    .Where(w => !w.CourseName.IsNullOrEmpty())
                    .OrderBy(o => o.CourseNumber)
                    .ToList();

                // создаем результирующую модель представления
                var resultCourseScheduleViewModel = new CourseScheduleViewModel
                {
                    CoursesViewModel = sortedCoursesViewModel,
                    NameOfDayWeek = input.FirstOrDefault()?.NameOfDayWeek?.ToUpper(),
                    DatetimeRequest = dateTimeRequest.Date.ToShortDateString(),
                    UpdateDatetime = _fileService.GetLastWriteTime().Result.Date.ToShortDateString()
                };

                return resultCourseScheduleViewModel;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // проверяем свежесть файла
                var isNewFile = _fileService.CheckCurrentExcelFile(DateTimeOffset.UtcNow);

                // TODO: вынести в конфиг
                var downloadFileParametersModel = new DownloadFileParametersModel
                {
                    PathToFile = Path.Combine("Infrastructure", "ScheduleFile"),
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/3d4/"),
                    FileNameLocal = "scheduleFile",
                    FileNameRemote = "KBiSP-3-kurs-2-sem",
                    FileFormat = "xlsx"
                };

                // если не свежий => качаем новый (1 сутки)
                if (!isNewFile.Result)
                    await _fileService.DownloadAsync(
                    downloadFileParametersModel, cancellationToken);

                // берем лист из excel файла
                //  var courseScheduleDatabaseModels = _courseScheduleFileService.GetFromExcel();

                // отправляем запрос на сохранения данных в бд (возможно, такого функционала и не появится)
                //  await _courseScheduleDatabaseService.UpdateAsync(courseScheduleDatabaseModels, cancellationToken);

            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public Task<CourseScheduleUpdateResultModel> GetLastAccessTimeUtc() => Task.Run(() =>
        {
            try
            {
                var lastAccessTimeUtc = _fileService.GetLastWriteTime();

                //https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastwritetime
                var errorDatetime = new DateTime(1601, 01, 01, 3, 0, 0);

                return new CourseScheduleUpdateResultModel
                {
                    UpdateDatetime = lastAccessTimeUtc.Result == errorDatetime
                        ? "Неизвестно"
                        : lastAccessTimeUtc.Result.ToShortDateString()
                };
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        });
    }
}
