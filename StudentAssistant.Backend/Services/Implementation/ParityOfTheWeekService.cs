using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ConfigurationModels;
using StudentAssistant.Backend.Models.ViewModels;
using Humanizer;
using StudentAssistant.Backend.Models.ParityOfTheWeek;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ParityOfTheWeekService : IParityOfTheWeekService
    {
        private readonly IMapper _mapper;
        private readonly ParityOfTheWeekConfigurationModel _config;

        public ParityOfTheWeekService(IMapper mapper, ParityOfTheWeekConfigurationModel config)
        {
            _mapper = mapper;
            _config = config;
        }

        public bool GetParityOfTheWeekByDateTime(DateTime timeNowParam)
        {
            if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

            var timeNow = timeNowParam;

            int firstDayOfStudy = (int)new DateTime(timeNow.Year, 9, 1).DayOfWeek;
            int weekNumber = (timeNow.DayOfYear + firstDayOfStudy) / 7 + 1;

            if (weekNumber % 2 == 0)
            {
                return true;
            }

            return false;
        }

        public ParityOfTheWeekModel GenerateDataOfTheWeek(DateTime timeNowParam)
        {
            try
            {
                if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

                var timeNow = timeNowParam;

                var parityOfTheWeekModel = new ParityOfTheWeekModel
                {
                    DateTimeRequest = timeNow,
                    ParityOfWeekToday = GetParityOfTheWeekByDateTime(timeNow),
                    ParityOfWeekCount = GetCountParityOfWeek(timeNow),
                    PartOfSemester = GetPartOfSemester(timeNow),
                    NumberOfSemester = GetNumberOfSemester(timeNow, _config.StartLearningYear),
                    DayOfName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(timeNow.DayOfWeek),
                    StatusDay = GetStatusDay(timeNow)
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
        /// <param name="timeNowParam"></param>
        /// <param name="holidaysDays"></param>
        /// <returns></returns>
        private StatusDayType GetStatusDay(DateTime timeNowParam, List<DateTime> holidaysDays = null)
        {
            if (holidaysDays != null)
            {
                if (holidaysDays.Contains(timeNowParam)) // определяет, является ли дата праздничной
                    return StatusDayType.HolidayWeekend;
            }

            if (timeNowParam.Month == 1) // зимняя сессия - январь
                return StatusDayType.ExamsTime;

            if (timeNowParam.Month == 6) // летняя сессия - июнь
                return StatusDayType.ExamsTime;

            if (timeNowParam.Month == 12 && (timeNowParam.Day >= 24 && timeNowParam.Day <= 31)) // зимняя зачетная сессия - последняя неделя декабря
                return StatusDayType.ExamsTime;

            if (timeNowParam.Month == 5 && (timeNowParam.Day >= 24 && timeNowParam.Day <= 31)) // летняя сессия - последняя неделя мая
                return StatusDayType.ExamsTime;

            if (timeNowParam.Month == 2 && timeNowParam < new DateTime(timeNowParam.Year, 2, 8)) // первая неделя февраля - каникулы
                return StatusDayType.Holiday;

            if (timeNowParam.Month >= 7 && timeNowParam.Month <= 8) // летние каникулы - июль-август
                return StatusDayType.Holiday;

            if (IsHoliday(timeNowParam)) // если это выходной
                return StatusDayType.DayOff;

            return StatusDayType.SchoolDay;
        }

        /// <summary>
        /// Возвращает <see cref="true"/>, если переданная дата выпадает на выходной день, иначе <see cref="false"/>.
        /// </summary>
        /// <param name="timeNowParam"></param>
        /// <param name="isSixDayWorkingWeek"></param>
        /// <returns></returns>
        private bool IsHoliday(DateTime timeNowParam, bool isSixDayWorkingWeek = false)
        {
            if (isSixDayWorkingWeek)
            {
                if (timeNowParam.Day == 7)
                {
                    return true;
                }
            }
            else
            {
                if (timeNowParam.Day == 6 || timeNowParam.Day == 7)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCountParityOfWeek(DateTime timeNowParam)
        {
            try
            {
                if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

                int result = 0;

                //if (timeNowParam.Month >= 7 && timeNowParam.Month <= 8) // обнуление идет с июля по август
                //    return result;

                if (GetPartOfSemester(timeNowParam) == 1)
                {
                    result = GetWeekNumberOfYear(timeNowParam)
                             - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 9, 1)); // сентябрь - начало уч. года - 1 семестр
                }
                else
                {
                    if (timeNowParam.Month == 1 && (GetStatusDay(timeNowParam) == StatusDayType.ExamsTime
                          || GetStatusDay(timeNowParam) == StatusDayType.Holiday)) // если зимняя сессия или каникулы, возвращает счетчик от начала года
                    {
                        result = GetWeekNumberOfYear(timeNowParam);
                    }
                    else
                    {
                        result = GetWeekNumberOfYear(timeNowParam)
                                                   - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 2, 8)); // февраль - начало 2 семестра
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

        public int GetWeekNumberOfYear(DateTime timeNowParam)
        {
            if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

            int weekNumber = CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(timeNowParam, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public int GetPartOfSemester(DateTime timeNowParam)
        {
            if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

            return timeNowParam.Month <= 12 && timeNowParam.Month >= 9 ? 1 : 2;
        }

        public int GetNumberOfSemester(DateTime timeNowParam, int startLearningYear)
        {
            if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

            if (GetPartOfSemester(timeNowParam) == 1)
                return (timeNowParam.Year - startLearningYear) * 2 + 1; // текущий год - год начала обучения = n лет обучения * 2 =
            // кол-во семестров +1 вначале учебного года

            return (timeNowParam.Year - startLearningYear) * 2;
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
