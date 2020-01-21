using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Models.ExamSchedule;
using StudentAssistant.Backend.Models.ExamSchedule.ViewModels;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ExamScheduleService : IExamScheduleService
    {
        private readonly IExamScheduleDatabaseService _examScheduleDatabaseService;
        private readonly ICourseScheduleFileService _courseScheduleFileService;
        private readonly ILogger<ExamScheduleService> _logger;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        private readonly string _fileName = Path.Combine("Infrastructure", "ScheduleFile", "examScheduleFile.xls");

        public ExamScheduleService(
            ICourseScheduleFileService courseScheduleFileService,
            ILogger<ExamScheduleService> logger,
            IFileService fileService,
            IMapper mapper, IExamScheduleDatabaseService examScheduleDatabaseService)
        {
            _courseScheduleFileService = courseScheduleFileService ??
                                         throw new ArgumentNullException(nameof(courseScheduleFileService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _examScheduleDatabaseService = examScheduleDatabaseService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExamScheduleViewModel> Get(ExamScheduleRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation("Get: " + $"{requestModel?.GroupName}");

               // var examScheduleList = await _courseScheduleFileService.GetExamScheduleFromExcelFile(_fileName);

                var parameters = new ExamScheduleParametersModel()
                {
                    CourseType = CourseType.ExamCourse,
                    StudyGroupModel = new StudyGroupModel() { Name = requestModel.GroupName }
                };

                var examScheduleList = await _examScheduleDatabaseService.GetByParameters(parameters);

                var examScheduleViewModel = PrepareViewModel(examScheduleList, requestModel);

                return examScheduleViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetExamSchedule Exception: " + ex);
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        private ExamScheduleViewModel PrepareViewModel(
            List<ExamScheduleDatabaseModel> examScheduleList,
            ExamScheduleRequestModel requestModel)
        {
            // маппим список предметов из бд в модель представления
            var controlCourseViewModel = _mapper.Map<List<ExamCourseViewModel>>(examScheduleList);

            // удаляем пустые предметы и сортируем по позиции в раписании
            var sortedControlCourseViewModel = controlCourseViewModel
                .Where(w => !string.IsNullOrEmpty(w.CourseName)
                            && string.Equals(w.StudyGroupModel.Name, requestModel.GroupName)
                )
                .OrderBy(o => Int32.Parse(o.NumberDate))
                .ToList();

            // создаем результирующую модель представления
            var resultControlWeekViewModel = new ExamScheduleViewModel
            {
                ExamCourseViewModel = sortedControlCourseViewModel,
                DatetimeRequest = DateTimeOffset.UtcNow.Date.ToShortDateString(),
                UpdateDatetime = _fileService.GetLastWriteTime(_fileName).Result.Date.ToShortDateString()
            };

            _logger.LogInformation("PrepareViewModel: "
                                   + "ExamViewModel: " + "DatetimeRequest: " +
                                   resultControlWeekViewModel.DatetimeRequest + " " +
                                   "ExamCourseViewModel.Count: " + resultControlWeekViewModel.ExamCourseViewModel.Count
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
                    //https://www.mirea.ru/upload/medialibrary/272/ekz_KBiSP_4-kurs_zima.xls
                    PathToFile = Path.Combine("Infrastructure", "ScheduleFile"),
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/272/"),
                    FileNameLocal = "examScheduleFile",
                    FileNameRemote = "ekz_KBiSP_4-kurs_zima",
                    FileFormat = "xls"
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
                case "пн":
                    return DayOfWeek.Monday;
                case "вт":
                    return DayOfWeek.Tuesday;
                case "ср":
                    return DayOfWeek.Wednesday;
                case "чт":
                    return DayOfWeek.Thursday;
                case "пт":
                    return DayOfWeek.Friday;
                case "сб":
                    return DayOfWeek.Saturday;
                case "вс":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("UpdateAsync: " + "Start");

                var examScheduleList = await _courseScheduleFileService.GetExamScheduleFromExcelFile(_fileName);

                var examScheduleDatabaseModels = examScheduleList.Where(w => !string.IsNullOrEmpty(w.CourseName)).ToList();

                await _examScheduleDatabaseService.UpdateAsync(examScheduleDatabaseModels, cancellationToken);
               
              // await _examScheduleDatabaseService.InsertAsync(examScheduleList, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateAsync Exception: " + ex);
                throw new NotSupportedException();
            }
        }
    }
}