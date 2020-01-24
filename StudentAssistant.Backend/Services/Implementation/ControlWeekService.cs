using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentAssistant.Backend.Helpers;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Models.UpdateAsync;
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
            IControlWeekDatabaseService controlWeekDatabaseService,
            ICourseScheduleFileService courseScheduleFileService,
            ILogger<CourseScheduleService> logger,
            IFileService fileService,
            IMapper mapper)
        {
            _controlWeekDatabaseService = controlWeekDatabaseService;
            _courseScheduleFileService = courseScheduleFileService;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ControlWeekViewModel> Get(ControlWeekRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("Get: " + $"{requestModel?.GroupName}");

                var controlWeekList = await _controlWeekDatabaseService.Get(cancellationToken);

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
                            && string.Equals(w.StudyGroupModel?.Name, requestModel.GroupName)
                )
                .Select(s =>
                {
                    s.NameOfDayWeek = StringConverterHelper.UppercaseFirst(s.NameOfDayWeek);
                    return s;
                })
                .OrderBy(o => StringConverterHelper.ToDayOfWeek(o.NameOfDayWeek))
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
        
        public async Task<UpdateAsyncResponseModel> UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("UpdateAsync: " + "Start");

                var courseScheduleList = await _courseScheduleFileService.GetFromExcelFile(_fileName);

                await _controlWeekDatabaseService.UpdateAsync(courseScheduleList, cancellationToken);

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

        public async Task InsertAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("InsertAsync: " + "Start");

                var courseScheduleList = await _courseScheduleFileService.GetFromExcelFile(_fileName);

                await _controlWeekDatabaseService.InsertAsync(courseScheduleList, cancellationToken);
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

                _controlWeekDatabaseService.MarkLikeDeleted();
            }
            catch (Exception ex)
            {
                _logger.LogError("MarkLikeDeleted Exception: " + ex);
                throw new NotSupportedException();
            }
        }
    }
}