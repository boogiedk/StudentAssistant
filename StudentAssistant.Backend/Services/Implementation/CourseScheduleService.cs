﻿using AutoMapper;
using StudentAssistant.Backend.Models.CourseSchedule;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Services.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services.Interfaces;

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

        public CourseScheduleViewModel Get(CourseScheduleDtoModel input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            try
            {
                // подготавливаем параметры для получения расписания
                var courseScheduleParameters = new CourseScheduleParameters
                {
                    NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(input.DateTimeRequest),
                    NameOfDayWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(input.DateTimeRequest.DayOfWeek),
                    ParityWeek = _parityOfTheWeekService.GetParityOfTheWeekByDateTime(input.DateTimeRequest),
                    GroupName = input.GroupName,
                    DatetimeRequest = input.DateTimeRequest
                };

                // на данным момент расписание берется из Excel файла.
                var courseScheduleDatabaseModel = _courseScheduleFileService
                   .GetFromExcelFileByParameters(courseScheduleParameters);

                var courseScheduleModel = _mapper.Map<List<CourseScheduleModel>>(courseScheduleDatabaseModel);

                var result = PrepareViewModel(courseScheduleModel, courseScheduleParameters);

                return result;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        private CourseScheduleViewModel PrepareViewModel(
            IEnumerable<CourseScheduleModel> input, CourseScheduleParameters parameters)
        {
            if (input == null || parameters == null) throw new ArgumentNullException(nameof(input));

            try
            {
                // если отсутствуют данные о расписании, возвращаем пустую модель
                if (!input.Any())
                {
                    var emptyCourseScheduleViewModel = new CourseScheduleViewModel
                    {
                        NameOfDayWeek = parameters.NameOfDayWeek.ToUpper(), //input.FirstOrDefault()?.NameOfDayWeek?.ToUpper(),
                        DatetimeRequest = parameters.DatetimeRequest.Date.ToShortDateString(),
                        UpdateDatetime = _fileService.GetLastWriteTime().Result.ToShortDateString(),
                        CoursesViewModel = new List<CourseViewModel> { new CourseViewModel() },
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
                    UpdateDatetime = _fileService.GetLastWriteTime().Result.Date.ToShortDateString(),
                    NumberWeek = _parityOfTheWeekService.GetCountParityOfWeek(parameters.DatetimeRequest)
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
                var isNewFile = _fileService.CheckExcelFile(DateTime.UtcNow);

                // TODO: вынести в конфиг
                var downloadFileParametersModel = new DownloadFileParametersModel
                {
                    //https://www.mirea.ru/upload/medialibrary/a72/KBiSP-4-kurs-1-sem.xlsx
                    PathToFile = Path.Combine("Infrastructure", "ScheduleFile"),
                    RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/a72/"),
                    FileNameLocal = "scheduleFile",
                    FileNameRemote = "KBiSP-4-kurs-1-sem",
                    FileFormat = "xlsx"
                };

                // если не свежий => качаем новый (1 сутки)
                if (!isNewFile.Result)
                    await _fileService.DownloadAsync(
                    downloadFileParametersModel, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public async Task UpdateByLinkAsync(
            CourseScheduleUpdateByLinkAsyncModel request,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if(request == null) return;

                    await _fileService.DownloadByLinkAsync(request.Uri,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public Task<CourseScheduleUpdateResponseModel> GetLastAccessTimeUtc() => Task.Run(() =>
        {
            try
            {
                var lastAccessTimeUtc = _fileService.GetLastWriteTime();

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
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        });
    }
}
