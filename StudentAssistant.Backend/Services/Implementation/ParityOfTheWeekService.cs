﻿using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ConfigurationModels;
using StudentAssistant.Backend.Models.ViewModels;
using Humanizer;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using Microsoft.Extensions.Options;

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

        public bool GetParityOfTheWeekByDateTime(DateTime timeNowParam)
        {
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
                var timeNow = timeNowParam;

                var parityOfTheWeekModel = new ParityOfTheWeekModel
                {
                    DateTimeRequest = timeNow,
                    ParityOfWeekToday = GetParityOfTheWeekByDateTime(timeNow),
                    ParityOfWeekCount = GetCountParityOfWeek(timeNow),
                    PartOfSemester = GetPartOfSemester(timeNow),
                    NumberOfSemester = GetNumberOfSemester(timeNow, _parityOfTheWeekConfigurationModel.StartLearningYear),
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
                // определяет, является ли дата праздничной
                if (holidaysDays.Contains(timeNowParam)) 
                    return StatusDayType.HolidayWeekend;
            }

            // зимняя сессия - январь
            if (timeNowParam.Month == 1)
                return StatusDayType.ExamsTime;

            // летняя сессия - июнь
            if (timeNowParam.Month == 6) 
                return StatusDayType.ExamsTime;

            // зимняя зачетная сессия - последняя неделя декабря
            if (timeNowParam.Month == 12 && (timeNowParam.Day >= 24 && timeNowParam.Day <= 31)) 
                return StatusDayType.ExamsTime;

            // летняя сессия - последняя неделя мая
            if (timeNowParam.Month == 5 && (timeNowParam.Day >= 24 && timeNowParam.Day <= 31)) 
                return StatusDayType.ExamsTime;

            // первая неделя февраля - каникулы (сколько длятся каникулы?)
            if (timeNowParam.Month == 2 && timeNowParam < new DateTime(timeNowParam.Year, 2, 9)) 
                return StatusDayType.Holiday;

            // летние каникулы - июль-август
            if (timeNowParam.Month >= 7 && timeNowParam.Month <= 8) 
                return StatusDayType.Holiday;

            // если это выходной
            if (IsHoliday(timeNowParam)) 
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
                if (timeNowParam.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
            }
            else
            {
                if (timeNowParam.DayOfWeek == DayOfWeek.Saturday || timeNowParam.DayOfWeek == DayOfWeek.Sunday)
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
                int result = 0;

                if (GetPartOfSemester(timeNowParam) == 1)
                {
                    // сентябрь - начало уч. года - 1 семестр
                    result = GetWeekNumberOfYear(timeNowParam)
                             - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 9, 1)); 
                }
                else
                {
                    // если зимняя сессия или каникулы, возвращает счетчик от начала года
                    if (GetStatusDay(timeNowParam) != StatusDayType.SchoolDay) 
                    {
                        result = GetWeekNumberOfYear(timeNowParam);
                    }
                    else
                    {
                        // февраль - начало 2 семестра
                        result = GetWeekNumberOfYear(timeNowParam)
                                                   - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 2, 8)); 
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
            int weekNumber = CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(timeNowParam, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public int GetPartOfSemester(DateTime timeNowParam)
        {
            return timeNowParam.Month <= 12 && timeNowParam.Month >= 9 ? 1 : 2;
        }

        public int GetNumberOfSemester(DateTime timeNowParam, int startLearningYear)
        {
            // текущий год - год начала обучения = n лет обучения * 2 =
            // кол-во семестров +1 вначале учебного года
            if (GetPartOfSemester(timeNowParam) == 1)
                return (timeNowParam.Year - startLearningYear) * 2 + 1; 

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
