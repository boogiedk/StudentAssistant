using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Humanizer;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using Microsoft.Extensions.Options;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ParityOfTheWeekService : IParityOfTheWeekService
    {
        private readonly IMapper _mapper;
        private readonly ParityOfTheWeekConfigurationModel _parityOfTheWeekConfigurationModel;

        public ParityOfTheWeekService(IMapper mapper, IOptions<ParityOfTheWeekConfigurationModel> parityOfTheWeekConfigurationModel)
        {
            _mapper = mapper;
            _parityOfTheWeekConfigurationModel = parityOfTheWeekConfigurationModel.Value;
        }

        public bool GetParityOfTheWeekByDateTime(DateTimeOffset dateTimeOffsetParam)
        {
            var timeNow = dateTimeOffsetParam;

            int firstDayOfStudy = (int)new DateTime(timeNow.Year, 9, 1).DayOfWeek;
            int weekNumber = (timeNow.DayOfYear + firstDayOfStudy) / 7 + 1;

            if (weekNumber % 2 == 0)
            {
                return true;
            }

            return false;
        }

        public ParityOfTheWeekModel GenerateDataOfTheWeek(DateTimeOffset dateTimeOffsetParam)
        {
            try
            {
                var dateTimeOffsetRequest = dateTimeOffsetParam;

                var parityOfTheWeekModel = new ParityOfTheWeekModel
                {
                    DateTimeRequest = dateTimeOffsetRequest,
                    ParityOfWeekToday = GetParityOfTheWeekByDateTime(dateTimeOffsetRequest),
                    ParityOfWeekCount = GetCountParityOfWeek(dateTimeOffsetRequest),
                    PartOfSemester = GetPartOfSemester(dateTimeOffsetRequest),
                    NumberOfSemester = GetNumberOfSemester(dateTimeOffsetRequest, _parityOfTheWeekConfigurationModel.StartLearningYear),
                    DayOfName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTimeOffsetRequest.DayOfWeek),
                    StatusDay = GetStatusDay(dateTimeOffsetRequest)
                };

                return parityOfTheWeekModel;
            }
            catch (Exception ex)
            {
                //log
                throw new NotSupportedException($"Ошибка во время выполнения: {ex}");
            }
        }

        /// <summary>
        /// Возвращает статус указанного дня.
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <param name="holidaysDays"></param>
        /// <returns></returns>
        private StatusDayType GetStatusDay(DateTimeOffset dateTimeOffsetParam, List<DateTimeOffset> holidaysDays = null)
        {
            if (holidaysDays != null)
            {
                // определяет, является ли дата праздничной
                if (holidaysDays.Contains(dateTimeOffsetParam))
                    return StatusDayType.HolidayWeekend;
            }

            // зимняя сессия - январь
            if (dateTimeOffsetParam.Month == 1)
                return StatusDayType.ExamsTime;

            // летняя сессия - июнь
            if (dateTimeOffsetParam.Month == 6)
                return StatusDayType.ExamsTime;

            // зимняя зачетная сессия - последняя неделя декабря
            if (dateTimeOffsetParam.Month == 12 && (dateTimeOffsetParam.Day >= 24 && dateTimeOffsetParam.Day <= 31))
                return StatusDayType.ExamsTime;

            // летняя сессия - последняя неделя мая
            if (dateTimeOffsetParam.Month == 5 && (dateTimeOffsetParam.Day >= 24 && dateTimeOffsetParam.Day <= 31))
                return StatusDayType.ExamsTime;

            // первая неделя февраля - каникулы (сколько длятся каникулы?)
            if (dateTimeOffsetParam.Month == 2 && dateTimeOffsetParam < new DateTime(dateTimeOffsetParam.Year, 2, 9))
                return StatusDayType.Holiday;

            // летние каникулы - июль-август
            if (dateTimeOffsetParam.Month >= 7 && dateTimeOffsetParam.Month <= 8)
                return StatusDayType.Holiday;

            // выходной
            if (IsHoliday(dateTimeOffsetParam))
                return StatusDayType.DayOff;

            return StatusDayType.SchoolDay;
        }

        /// <summary>
        /// Возвращает <see cref="true"/>, если переданная дата выпадает на выходной день, иначе <see cref="false"/>.
        /// </summary>
        /// <param name="dateTimeOffsetParam"></param>
        /// <param name="isSixDayWorkingWeek">5-ти или 6-ти дневная рабочая неделя.</param>
        /// <returns></returns>
        private bool IsHoliday(DateTimeOffset dateTimeOffsetParam, bool isSixDayWorkingWeek = false)
        {
            if (isSixDayWorkingWeek)
            {
                if (dateTimeOffsetParam.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
            }
            else
            {
                if (dateTimeOffsetParam.DayOfWeek == DayOfWeek.Saturday || dateTimeOffsetParam.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCountParityOfWeek(DateTimeOffset dateTimeOffsetParam)
        {
            try
            {
                int result = 0;

                if (GetPartOfSemester(dateTimeOffsetParam) == 1)
                {
                    // сентябрь - начало уч. года - 1 семестр
                    result = GetWeekNumberOfYear(dateTimeOffsetParam)
                             - GetWeekNumberOfYear(new DateTime(dateTimeOffsetParam.Year, 9, 1));
                }
                else
                {
                    var currentStatusDay = GetStatusDay(dateTimeOffsetParam);

                    // если зимняя сессия или каникулы, возвращает счетчик от начала года
                    if (currentStatusDay == StatusDayType.ExamsTime
                        || currentStatusDay == StatusDayType.Holiday)
                    {
                        result = GetWeekNumberOfYear(dateTimeOffsetParam);
                    }
                    else
                    {
                        // февраль - начало 2 семестра
                        result = GetWeekNumberOfYear(dateTimeOffsetParam)
                                                   - GetWeekNumberOfYear(new DateTime(dateTimeOffsetParam.Year, 2, 8));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                //log
                throw new NotSupportedException($"Ошибка во время выполнения: {ex}");
            }
        }

        public int GetWeekNumberOfYear(DateTimeOffset dateTimeOffsetParam)
        {
            int weekNumber = CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(dateTimeOffsetParam.DateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public int GetPartOfSemester(DateTimeOffset dateTimeOffsetParam)
        {
            return dateTimeOffsetParam.Month <= 12 && dateTimeOffsetParam.Month >= 9 ? 1 : 2;
        }

        public int GetNumberOfSemester(DateTimeOffset dateTimeOffsetParam, int startLearningYear)
        {
            // текущий год - год начала обучения = n лет обучения * 2 =
            // кол-во семестров +1 вначале учебного года
            if (GetPartOfSemester(dateTimeOffsetParam) == 1)
                return (dateTimeOffsetParam.Year - startLearningYear) * 2 + 1;

            return (dateTimeOffsetParam.Year - startLearningYear) * 2;
        }

        public ParityOfTheWeekViewModel PrepareParityOfTheWeekViewModel(ParityOfTheWeekModel input)
        {
            if (input == null) throw new NotSupportedException($"{typeof(ParityOfTheWeekModel)} input равен null");

            var resultViewModel = _mapper.Map<ParityOfTheWeekViewModel>(input);

            resultViewModel.ParityOfWeekToday = input.ParityOfWeekToday ? "Чётная" : "Нечётная";
            resultViewModel.DateTimeRequest = input.DateTimeRequest.ToString("D");
            resultViewModel.StatusDay = input.StatusDay.Humanize();
            resultViewModel.IsParity = input.ParityOfWeekToday;

            return resultViewModel;
        }
    }
}
