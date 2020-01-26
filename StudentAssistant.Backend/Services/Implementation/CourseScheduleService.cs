using AutoMapper;
using StudentAssistant.Backend.Models.CourseSchedule;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Models.UpdateAsync;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using CourseScheduleDtoModel = StudentAssistant.Backend.Models.CourseSchedule.CourseScheduleDtoModel;
using CourseType = StudentAssistant.Backend.Models.CourseSchedule.CourseType;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleDatabaseService _courseScheduleDatabaseService;
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly IParityOfTheWeekService _parityOfTheWeekService;
        private readonly ILogger<CourseScheduleService> _logger;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        private readonly string _fileName = Path.Combine("Infrastructure", "ScheduleFile", "scheduleFile.xlsx");

        public CourseScheduleService(
            ICourseScheduleDatabaseService courseScheduleDatabaseService,
            ICourseScheduleFileService courseScheduleFileService,
            IParityOfTheWeekService parityOfTheWeekService,
            ILogger<CourseScheduleService> logger,
            IFileService fileService,
            IMapper mapper)
        {
            _courseScheduleFileService = courseScheduleFileService ??
                                         throw new ArgumentNullException(nameof(courseScheduleFileService));
            _parityOfTheWeekService =
                parityOfTheWeekService ?? throw new ArgumentNullException(nameof(parityOfTheWeekService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _courseScheduleDatabaseService = courseScheduleDatabaseService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CourseScheduleViewModel> Get(CourseScheduleDtoModel input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            _logger.LogInformation("Get: " +
                                   "DatetimeRequest: " + input.DateTimeRequest + " " +
                                   "GroupName: " + input.GroupName);

            try
            {
                // подготавливаем параметры для получения расписания
                var courseScheduleParameters = new CourseScheduleParameters
                {
                    NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(input.DateTimeRequest),
                    NameOfDayWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat
                        .GetDayName(input.DateTimeRequest.DayOfWeek),
                    ParityWeek = _parityOfTheWeekService.GetParityOfTheWeekByDateTime(input.DateTimeRequest),
                    GroupName = input.GroupName,
                    DatetimeRequest = input.DateTimeRequest,
                    FileName = _fileName
                };

                var courseScheduleDatabaseModel =
                    await _courseScheduleDatabaseService.GetByParameters(courseScheduleParameters);

                var courseScheduleModel = _mapper.Map<List<CourseScheduleModel>>(courseScheduleDatabaseModel);

                var result = PrepareViewModel(courseScheduleModel, courseScheduleParameters);

                _logger.LogInformation("Get: " + result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Get Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        private CourseScheduleViewModel PrepareViewModel(
            List<CourseScheduleModel> input,
            CourseScheduleParameters parameters)
        {
            if (input == null || parameters == null) throw new ArgumentNullException(nameof(input));

            try
            {
                _logger.LogInformation("PrepareViewModel: " +
                                       "CourseScheduleModel: " + input.Count + " " +
                                       "CourseScheduleParameters: " + parameters.DatetimeRequest + " " +
                                       "GroupName: " + parameters.GroupName
                );

                // если отсутствуют данные о расписании, возвращаем пустую модель
                if (IsEmptyCourseSchedule(input))
                {
                    var emptyCourseScheduleViewModel = new CourseScheduleViewModel
                    {
                        NameOfDayWeek =
                            parameters.NameOfDayWeek.ToUpper(),
                        DatetimeRequest = parameters.DatetimeRequest.Date.ToShortDateString(),
                        UpdateDatetime = _fileService.GetLastWriteTime(parameters.FileName).Result.ToShortDateString(),
                        CoursesViewModel = new List<CourseViewModel> {new CourseViewModel()},
                        NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(parameters.DatetimeRequest.Date)
                    };

                    return emptyCourseScheduleViewModel;
                }

                // маппим список предметов из бд в модель представления
                var coursesViewModel = _mapper.Map<List<CourseViewModel>>(input);

                // удаляем пустые предметы и сортируем по позиции в раписании
                var sortedCoursesViewModel = coursesViewModel
                    .Where(w => !string.IsNullOrEmpty(w.CourseName))
                    .OrderBy(o => o.CourseNumber)
                    .ToList();

                // создаем результирующую модель представления
                var resultCourseScheduleViewModel = new CourseScheduleViewModel
                {
                    CoursesViewModel = sortedCoursesViewModel,
                    NameOfDayWeek = parameters.NameOfDayWeek.ToUpper(),
                    DatetimeRequest = parameters.DatetimeRequest.Date.ToShortDateString(),
                    UpdateDatetime = _fileService.GetLastWriteTime(parameters.FileName).Result.Date.ToShortDateString(),
                    NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(parameters.DatetimeRequest)
                };

                _logger.LogInformation("PrepareViewModel: "
                                       + "CoursesViewModel: " + "DatetimeRequest: " +
                                       resultCourseScheduleViewModel.DatetimeRequest + " " +
                                       "CoursesViewModel.Count: " + resultCourseScheduleViewModel.CoursesViewModel.Count
                );

                return resultCourseScheduleViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PrepareViewModel Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        private bool IsEmptyCourseSchedule(List<CourseScheduleModel> input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            if (!input.Any())
            {
                return true;
            }

            var counterEmpty = input.Count(courseScheduleModel =>
                string.Equals(courseScheduleModel.CourseName, string.Empty)
                && string.Equals(courseScheduleModel.CoursePlace, string.Empty));

            return counterEmpty == input.Count;
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
                    //https://www.mirea.ru/upload/medialibrary/a72/KBiSP-4-kurs-1-sem.xlsx
                    PathToFile = Path.Combine("Infrastructure", "ScheduleFile"),
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/0b8/"),
                    FileNameLocal = "scheduleFile",
                    FileNameRemote = "KBiSP-4-kurs-1-sem",
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
                    result.Message = "Обновление невозможно. Попробуйте позже.";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception DownloadAsync: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public async Task DownloadByLinkAsync(
            CourseScheduleUpdateByLinkAsyncModel request,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (request == null) return;

                _logger.LogInformation("DownloadByLinkAsync: " + "Uri: " + request.Uri);

                await _fileService.DownloadByLinkAsync(request.Uri, _fileName,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("DownloadByLinkAsync Exception : " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public async Task<UpdateAsyncResponseModel> UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("UpdateAsync: " + "Start");

                var courseScheduleList = await _courseScheduleFileService.GetFromExcelFile(_fileName);

                var courseScheduleDatabaseModels = courseScheduleList
                    .Where(w => !string.IsNullOrEmpty(w.CourseName))
                    .ToList();

                await  _courseScheduleDatabaseService.UpdateAsync(courseScheduleDatabaseModels,cancellationToken);

                var response = new UpdateAsyncResponseModel
                {
                    Message = "Данные обновлены!"
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateAsync Exception: " + ex);
                throw new NotSupportedException();
            }
        }

        public Task<CourseScheduleUpdateResponseModel> GetLastAccessTimeUtc() => Task.Run(() =>
        {
            try
            {
                _logger.LogInformation("GetLastAccessTimeUtc: " + "Start");


                var lastAccessTimeUtc = _fileService.GetLastWriteTime(_fileName);

                //https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastwritetime
                var errorDatetime = new DateTime(1601, 01, 01, 3, 0, 0);

                return new CourseScheduleUpdateResponseModel
                {
                    UpdateDatetime = lastAccessTimeUtc.Result == errorDatetime
                        ? "Неизвестно"
                        : lastAccessTimeUtc.Result.ToShortDateString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetLastAccessTimeUtc Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        });
        
        public async Task InsertAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("InsertAsync: " + "Start");

                var courseScheduleList = await _courseScheduleFileService.GetFromExcelFile(_fileName);
                
                var courseScheduleDatabaseModels = courseScheduleList
                    .Select(s =>
                    {
                        if (string.Equals(s.CourseName,"Военная кафедра"))
                        {
                            s.CourseType = DbLayer.Models.CourseSchedule.CourseType.ControlCourse;
                        }

                        return s;
                    })
                    .Where(w => !string.IsNullOrEmpty(w.CourseName))
                    .ToList();

                await _courseScheduleDatabaseService.InsertAsync(courseScheduleDatabaseModels, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("InsertAsync Exception: " + ex);
                throw new NotSupportedException();
            }
        }

        public void MarkLikeDeleted()
        {
            try
            {
                _logger.LogInformation("MarkLikeDeleted: " + "Start");

                _courseScheduleDatabaseService.MarkLikeDeleted();
            }
            catch (Exception ex)
            {
                _logger.LogError("MarkLikeDeleted Exception: " + ex);
                throw new NotSupportedException();
            }
        }
        
    }
}