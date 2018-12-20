using System;
using System.Globalization;
using AutoMapper;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ConfigurationModels;
using StudentAssistant.Backend.Models.ViewModels;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class ParityOfTheWeekService : IParityOfTheWeekService
    {
        private readonly IMapper _mapper;
        private readonly ParityOfTheWeekConfigurationModel _config;

        public ParityOfTheWeekService(IMapper mapper
            , ParityOfTheWeekConfigurationModel config
            )
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
                    DayOfName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(timeNow.DayOfWeek)
                };

                return parityOfTheWeekModel;
            }
            catch(Exception ex)
            {
                //log
                throw new NotSupportedException($"Ошибка во время выполнения: {ex}");
            }
        }

        public int GetCountParityOfWeek(DateTime timeNowParam)
        {
            try
            {
                if (timeNowParam == null) throw new NotSupportedException($"{typeof(DateTime)} timeNowParam равен null");

                int result = 0;

                if (timeNowParam.Month >= 7 && timeNowParam.Month <= 8) // обнуление идет с июля по август
                    return result;

                if (GetPartOfSemester(timeNowParam) == 1)
                {
                    result = GetWeekNumberOfYear(timeNowParam)
                             - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 9, 1)); // сентябрь - начало уч. года - 1 семестр
                }
                else
                {
                    result = GetWeekNumberOfYear(timeNowParam)
                             - GetWeekNumberOfYear(new DateTime(timeNowParam.Year, 2, 1)); // февраль - начало 2 семестра
                }

                return result;
            }
            catch(Exception ex)
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
            if (input == null) throw new NotSupportedException($"{typeof(DateTime)} input раве null");

            var resultViewModel = _mapper.Map<ParityOfTheWeekViewModel>(input);

            resultViewModel.ParityOfWeekToday = input.ParityOfWeekToday ? "Чётная" : "Нечётная";
            resultViewModel.DateTimeRequest = input.DateTimeRequest.ToString("D");

            return resultViewModel;
        }
    }
}
