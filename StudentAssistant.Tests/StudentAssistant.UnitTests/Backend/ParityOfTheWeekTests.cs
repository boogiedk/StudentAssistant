using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>

    public class ParityOfTheWeekTests
    {
        private readonly Mock<IOptions<ParityOfTheWeekConfigurationModel>> _parityOfTheWeekConfigurationModel;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IMapper _mapper;

        public ParityOfTheWeekTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _parityOfTheWeekConfigurationModel =
                fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper(); 
           _mockMapper = fixture.Freeze<Mock<IMapper>>();
        }

        [Theory]
        [InlineData("11-11-2018", 1)]
        [InlineData("01-01-2019", 2)]
        [InlineData("02-28-2021", 2)]
        public void GetPartOfSemester_PartOFSemester_ReturnsExpectedValue(
            DateTime dateTime, int expected)
        {
            // Arrange
            var dateTimeTest = dateTime;  // new DateTime(2018, 11, 11);

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.GetPartOfSemester(dateTimeTest);

            // Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("11-11-2018", 10)]
        [InlineData("01-01-2019", 1)]
        [InlineData("02-28-2021", 2)]
        public void GetCountParityOfWeek_CountOfParity_ReturnsExpectedValue(
            DateTime dateTime, 
            int expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.GetCountParityOfWeek(dateTimeTest);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("11-11-2018", 2016, 5)]
        [InlineData("01-01-2019", 2019, 0)] // баг - исправить
        [InlineData("02-28-2021", 2020, 2)]
        public void GetNumberOfSemester_NumberOfSemester_ReturnsExpectedValue(
            DateTime dateTime, 
            int startLearningDateParam,
            int expected)
        {
            // Arrange
            var dateTimeTest = dateTime;
            var startLearningDate = startLearningDateParam;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.GetNumberOfSemester(dateTimeTest, startLearningDate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("11-11-2018", true)]
        [InlineData("01-01-2019", false)]
        [InlineData("02-28-2021", true)]
        public void GetParityOfTheWeekByDateTime_ParityBoolValue_ReturnsExpectedValue(
            DateTime dateTime,
            bool expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.GetParityOfTheWeekByDateTime(dateTimeTest);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("11-11-2018", 45)]
        [InlineData("01-01-2019", 1)]
        [InlineData("02-28-2021", 8)]
        public void GetWeekNumberOfYear_WeekOfYear_ReturnsExpectedValue(
            DateTime dateTime, 
            int expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object); ;
            var result = service.GetWeekNumberOfYear(dateTimeTest);

            // Assert
            Assert.Equal(expected, result);
        }

        /*[Theory]
        [MemberData(nameof(GetParityOfTheWeekConfigurationModel))]
        public void GenerateDataOfTheWeek_ParityOfTheWeekModel_ReturnsExpectedValue(
            DateTime dateTime, 
            IOptions<ParityOfTheWeekConfigurationModel> parityOfTheWeekConfigurationModel, 
            ParityOfTheWeekModel expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(parityOfTheWeekConfigurationModel, _mockMapper.Object);
            var result = service.GenerateDataOfTheWeek(dateTimeTest);
            var isCompare = Compare(result, expected);

            // Assert
            Assert.True(isCompare);
        }*/

        [Theory]
        [InlineData("11-11-2018", StatusDayType.DayOff)]
        [InlineData("01-01-2019", StatusDayType.ExamsTime)]
        [InlineData("02-28-2021", StatusDayType.DayOff)]
        public void GetStatusDay_StatusDay_ReturnsExpectedValue(
            DateTime dateTime, 
            StatusDayType expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.GetStatusDay(dateTimeTest);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("11-11-2018", true, true)]
        [InlineData("01-01-2019", true, false)]
        [InlineData("02-28-2021", false, true)]
        public void IsHoliday_IsHolidayValue_ReturnsExpectedValue(
            DateTime dateTime,
            bool isSixDayWeek,
            bool expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(_parityOfTheWeekConfigurationModel.Object, _mockMapper.Object);
            var result = service.IsHoliday(dateTimeTest,isSixDayWeek);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetParityOfTheWeekModel))]
        public void PrepareViewModel_ViewModel_ReturnsExpectedValue(
           IOptions<ParityOfTheWeekConfigurationModel> parityOfTheWeekConfigurationModel,
            ParityOfTheWeekModel parityOfTheWeekModelParam,
            ParityOfTheWeekViewModel expected
            )
        {
            // Arrange
            var parityOfTheWeekModel = parityOfTheWeekModelParam;

            // Act
            var service = new ParityOfTheWeekService(parityOfTheWeekConfigurationModel, _mapper);
            var result = service.PrepareViewModel(parityOfTheWeekModel);
            var isCompare = Compare(result, expected);

            // Assert
            Assert.True(isCompare);
        }
        
        [Theory]
        [MemberData(nameof(GetParityOfTheWeekConfigurationModel))]
        public void GenerateDataOfTheWeek_ParityOfTheWeekModel_ReturnsExpectedValue(
            DateTime dateTime, 
            IOptions<ParityOfTheWeekConfigurationModel> parityOfTheWeekConfigurationModel, 
            ParityOfTheWeekModel expected)
        {
            // Arrange
            var dateTimeTest = dateTime;

            // Act
            var service = new ParityOfTheWeekService(parityOfTheWeekConfigurationModel, _mapper);
            var result = service.GenerateDataOfTheWeek(dateTimeTest);
            var isCompare = Compare(result, expected);

            // Assert
            Assert.True(isCompare);
        }

        #region Help Methods

        public static bool Compare<T>(T obj1, T obj2)
        {
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (obj1 == null || obj2 == null)
                return false;

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string object1Value = string.Empty;
                    string object2Value = string.Empty;

                    if (type.GetProperty(property.Name)?.GetValue(obj1, null) != null)
                        object1Value = type.GetProperty(property.Name)?.GetValue(obj1, null).ToString();
                    if (type.GetProperty(property.Name)?.GetValue(obj2, null) != null)
                        object2Value = type.GetProperty(property.Name)?.GetValue(obj2, null).ToString();
                    if (object2Value != null && (object1Value != null && object1Value.Trim() != object2Value.Trim()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static IEnumerable<object[]> GetParityOfTheWeekConfigurationModel()
        {
            var listOfObjects = new List<object[]>
            {
                new object[]
                {
                    new DateTime(2018, 11, 11),
                    Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2016}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2018, 11, 11)),
                        DayOfName = "воскресенье",
                        NumberOfSemester = 5,
                        ParityOfWeekCount = 10,
                        ParityOfWeekToday = true,
                        PartOfSemester = 1,
                        StatusDay = StatusDayType.DayOff
                    },
                },
                new object[]
                {
                    new DateTime(2021, 04, 29),
                    Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2017}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2021, 04, 29)),
                        DayOfName = "четверг",
                        NumberOfSemester = 8,
                        ParityOfWeekCount = 11,
                        ParityOfWeekToday = false,
                        PartOfSemester = 2,
                        StatusDay = StatusDayType.SchoolDay
                    },
                },
                new object[]
                {
                    new DateTime(2019, 1, 4),
                    Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2018}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2019, 1, 4)),
                        DayOfName = "пятница",
                        NumberOfSemester = 2,
                        ParityOfWeekCount = 1,
                        ParityOfWeekToday = false,
                        PartOfSemester = 2,
                        StatusDay = StatusDayType.ExamsTime
                    },
                }
            };

            return listOfObjects;
        }

        public static IEnumerable<object[]> GetParityOfTheWeekModel()
        {
            var listOfObjects = new List<object[]>
            {
                new object[]
                {
                    Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2016}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2018, 11, 11)),
                        DayOfName = "воскресенье",
                        NumberOfSemester = 5,
                        ParityOfWeekCount = 10,
                        ParityOfWeekToday = true,
                        PartOfSemester = 1,
                        StatusDay = StatusDayType.DayOff
                    },
                    new ParityOfTheWeekViewModel
                    {
                        DateTimeRequest =  new DateTimeOffset(new DateTime(2018, 11, 11)).ToString("D",new CultureInfo("ru-RU")),  //"11 ноября 2018 г.",
                        DayOfName = "воскресенье",
                        NumberOfSemester = 5,
                        ParityOfWeekCount = 10,
                        ParityOfWeekToday = "Чётная",
                        PartOfSemester = 1,
                        SelectedDateStringValue = "Выбрано",
                        StatusDay = "Выходной день",
                        IsParity = true
                    },
                },
                new object[]
                {
                    Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2017}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2021, 04, 29)),
                        DayOfName = "четверг",
                        NumberOfSemester = 8,
                        ParityOfWeekCount = 11,
                        ParityOfWeekToday = false,
                        PartOfSemester = 2,
                        StatusDay = StatusDayType.SchoolDay,
                    },
                    new ParityOfTheWeekViewModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2021, 04, 29)).ToString("D",new CultureInfo("ru-RU")),
                        DayOfName = "четверг",
                        NumberOfSemester = 8,
                        ParityOfWeekCount = 11,
                        ParityOfWeekToday = "Нечётная",
                        PartOfSemester = 2,
                        SelectedDateStringValue = "Выбрано",
                        StatusDay = "Учебный день",
                        IsParity = false
                    },
                },
                new object[]
                {
                   Options.Create(new ParityOfTheWeekConfigurationModel { StartLearningYear = 2018}),
                    new ParityOfTheWeekModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2019, 1, 4)),
                        DayOfName = "пятница",
                        NumberOfSemester = 2,
                        ParityOfWeekCount = 1,
                        ParityOfWeekToday = false,
                        PartOfSemester = 2,
                        StatusDay = StatusDayType.ExamsTime
                    },
                    new ParityOfTheWeekViewModel
                    {
                        DateTimeRequest = new DateTimeOffset(new DateTime(2019, 1, 4)).ToString("D",new CultureInfo("ru-RU")),
                        DayOfName = "пятница",
                        NumberOfSemester = 2,
                        ParityOfWeekCount = 1,
                        ParityOfWeekToday = "Нечётная",
                        PartOfSemester = 2,
                        SelectedDateStringValue = "Выбрано",
                        StatusDay = "Сессия",
                        IsParity = false
                    },
                }
            };

            return listOfObjects;
        }

        #endregion
    }
}

