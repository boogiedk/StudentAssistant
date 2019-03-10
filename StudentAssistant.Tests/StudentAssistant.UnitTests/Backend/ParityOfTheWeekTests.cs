using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Сценарий]_[Ожидаемое поведение]_[Результат]
    /// </summary>

    public class ParityOfTheWeekTests
    {
        [Fact]
        public void GetPartOfSemester_ReturnsPartOFSemester_ShouldBeReturns1()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var dateTimeTest = new DateTime(2018, 11, 11);

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetPartOfSemester(dateTimeTest);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Fact]
        public void GetCountParityOfWeek_ReturnsCountOfParity_ShouldBeReturns10()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var dateTimeTest = new DateTime(2018, 11, 11);

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetCountParityOfWeek(dateTimeTest);

            // Assert
            Assert.AreEqual(10, result);
        }

        [Fact]
        public void GetNumberOfSemester_ReturnsNumberOfSemester_ShouldBeReturns5()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var dateTimeTest = new DateTime(2018, 11, 11);

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            var startLearningDate = 2016;

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetNumberOfSemester(dateTimeTest, startLearningDate);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Fact]
        public void GetParityOfTheWeekByDateTime_ReturnsParityBoolValue_ShouldBeReturnsTrue()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var dateTimeTest = new DateTime(2018, 11, 11);

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetParityOfTheWeekByDateTime(dateTimeTest);

            // Assert
            Assert.IsTrue(result);
        }

        [Fact]
        public void GetWeekNumberOfYear_ReturnsWeekOfYear_ShouldBeReturns45()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var dateTimeTest = new DateTime(2018, 11, 11);

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetWeekNumberOfYear(dateTimeTest);

            // Assert
            Assert.AreEqual(45, result);
        }

        [Fact]
        public void GenerateDataOfTheWeek_ReturnsCorrectModel_ShouldBeReturnsParityOfTheWeekModel()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            var dateTimeTest = new DateTime(2018, 11, 11);

            var expectedModel = new ParityOfTheWeekModel
            {
                DateTimeRequest = dateTimeTest,
                DayOfName = "воскресенье",
                NumberOfSemester = 5,
                ParityOfWeekCount = 10,
                ParityOfWeekToday = true,
                PartOfSemester = 1
            };

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GenerateDataOfTheWeek(dateTimeTest);

            // Assert
            Assert.AreEqual(expectedModel.DateTimeRequest, result.DateTimeRequest);
        }

        [Fact]
        public void GetStatusDay_ReturnsCorrectStatusDay_ShouldBeReturnsStatusDay()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            var dateTimeTest = new DateTime(2018, 11, 11);

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.GetStatusDay(dateTimeTest);

            // Assert
            Assert.AreEqual(StatusDayType.DayOff,result);
        }

        [Fact]
        public void IsHoliday_ReturnsTrueOrFalse_ShouldBeReturnsTrue()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var mapper = fixture.Freeze<Mock<IMapper>>();
            var parityOfTheWeekConfigurationModel = fixture.Freeze<Mock<IOptions<ParityOfTheWeekConfigurationModel>>>();

            var dateTimeTest = new DateTime(2018, 11, 11);

            // Act
            var service = new ParityOfTheWeekService(mapper.Object, parityOfTheWeekConfigurationModel.Object);
            var result = service.IsHoliday(dateTimeTest);

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}

