using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ControlWeekService : IControlWeekService
    {
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IControlWeekDatabaseService _controlWeekDatabaseService;
        private readonly ILogger<CourseScheduleService> _logger;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        private readonly string _fileName = Path.Combine("Infrastructure", "ScheduleFile", "controlWeek.xlsx");

        public ControlWeekService(
            ICourseScheduleFileService courseScheduleFileService,
            ILogger<CourseScheduleService> logger,
            IFileService fileService,
            IMapper mapper, IControlWeekDatabaseService controlWeekDatabaseService)
        {
            _courseScheduleFileService = courseScheduleFileService ??
                                         throw new ArgumentNullException(nameof(courseScheduleFileService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _controlWeekDatabaseService = controlWeekDatabaseService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ControlWeekViewModel> Get(ControlWeekRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Get: " + $"{requestModel?.GroupName}");

              //  var controlWeekList = await _courseScheduleFileService.GetFromExcelFile(_fileName);

                var controlWeekList = await _controlWeekDatabaseService.Get();

                var controlWeekControlModel = PrepareViewModel(controlWeekList, requestModel);

                return controlWeekControlModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetControlWeek Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        private ControlWeekViewModel PrepareViewModel(
            List<CourseScheduleDatabaseModel> controlWeekList,
            ControlWeekRequestModel requestModel)
        {
            // маппим список предметов из бд в модель представления
            var controlCourseViewModel = _mapper.Map<List<ControlCourseViewModel>>(controlWeekList);

            // удаляем пустые предметы и сортируем по позиции в раписании
            var sortedControlCourseViewModel = controlCourseViewModel
                .Where(w => !string.IsNullOrEmpty(w.CourseName)
                            && w.CourseName != "Военная кафедра"
                            && string.Equals(w.StudyGroupModel.Name, requestModel.GroupName)
                )
                .Select(s =>
                {
                    s.NameOfDayWeek = UppercaseFirst(s.NameOfDayWeek);
                    return s;
                })
                .OrderBy(o => ToDayOfWeek(o.NameOfDayWeek))
                .ToList();

            // создаем результирующую модель представления
            var resultControlWeekViewModel = new ControlWeekViewModel
            {
                ControlCourseViewModel = sortedControlCourseViewModel,
                DatetimeRequest = DateTimeOffset.UtcNow.Date.ToShortDateString(),
                UpdateDatetime = _fileService.GetLastWriteTime(_fileName).Result.Date.ToShortDateString()
            };

            _logger.LogInformation("PrepareViewModel: "
                                   + "CoursesViewModel: " + "DatetimeRequest: " +
                                   resultControlWeekViewModel.DatetimeRequest + " " +
                                   "CoursesViewModel.Count: " + resultControlWeekViewModel.ControlCourseViewModel.Count
            );

            return resultControlWeekViewModel;
        }

        public async Task<DownloadAsyncResponseModel> DownloadAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("DownloadAsync: " + "Start");

                // проверяем свежесть файла
                var isNewFile = _fileService.CheckExcelFile(DateTime.UtcNow, _fileName);

                // TODO: вынести в конфиг
                var downloadFileParametersModel = new DownloadFileParametersModel
                {
                    //https://www.mirea.ru/upload/medialibrary/28e/zach_KBiSP_4-kurs_zima.xlsx
                    PathToFile = Path.Combine("Infrastructure", "ScheduleFile"),
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/28e/"),
                    FileNameLocal = "controlWeek",
                    FileNameRemote = "zach_KBiSP_4-kurs_zima",
                    FileFormat = "xlsx"
                };

                _logger.LogInformation("DownloadAsync: " + "isNewFile: " + await isNewFile);

                var result = new DownloadAsyncResponseModel
                {
                    IsNewFile = await isNewFile
                };


                // если не свежий => качаем новый (1 сутки)
                if (!(await isNewFile))
                {
                    await _fileService.DownloadAsync(
                        downloadFileParametersModel, cancellationToken);

                    result.Message = "Данные обновлены!";
                }
                else
                {
                    result.Message = "Обновление недоступно. Попробуйте позже.";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception DownloadAsync: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        /// <summary>
        /// Парсит строку с названием дня недели в DayOfWeek енум.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private DayOfWeek ToDayOfWeek(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new NullReferenceException();
            }

            switch (str.ToLower())
            {
                case "понедельник":
                    return DayOfWeek.Monday;
                case "вторник":
                    return DayOfWeek.Tuesday;
                case "среда":
                    return DayOfWeek.Wednesday;
                case "четверг":
                    return DayOfWeek.Thursday;
                case "пятница":
                    return DayOfWeek.Friday;
                case "суббота":
                    return DayOfWeek.Saturday;
                case "воскресенье":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Делает заглавной первую букву в слове (строке).
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        
        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("UpdateAsync: " + "Start");

                var courseScheduleList = await _courseScheduleFileService.GetFromExcelFile(_fileName);
                
                await _controlWeekDatabaseService.UpdateAsync(courseScheduleList, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateAsync Exception: " + ex);
                throw new NotSupportedException();
            }
        }
    }
}