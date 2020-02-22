using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services.Implementation;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.DbLayer
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class ImportDataExcelServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ImportDataExcelService>> _logger;

        public ImportDataExcelServiceTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _mockMapper = fixture.Freeze<Mock<IMapper>>();
            _logger = fixture.Freeze<Mock<ILogger<ImportDataExcelService>>>();
        }

        [Theory]
        [InlineData("10 чт", "10", "чт")]
        [InlineData("23 пн", "23", "пн")]
        [InlineData("5 ср", "5", "ср")]
        public void PrepareDate_TupleWithNumberDateAndDayOfWeek_SplitInputStringAndReturnTuple(
            string input,
            string expectedNumberDate, string expectedDayOfWeek)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            //Act
            var result = importDataExcelService.PrepareDate(input);

            //Assert
            Assert.Equal(expectedNumberDate, result.Item1);
            Assert.Equal(expectedDayOfWeek, result.Item2);
        }

        [Fact]
        public void GetExamScheduleDatabaseModels_CountDatabaseModels_ShouldParseExcelAndReturnCountModels()
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);
            var fileName = Path.Combine("TestFiles", "scheduleFileTest.xlsx");

            //Act
            var result = importDataExcelService.GetCourseScheduleDatabaseModels(fileName).ToList();

            //Assert
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void ParseExcelFileForThreeGroup_EnumerableModels_ShouldParseExcelAndReturnModels()
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);
            var fileName = Path.Combine("TestFiles", "examScheduleFileTest.xls");

            //Act
            var result = importDataExcelService.GetExamScheduleDatabaseModels(fileName).ToList();

            //Assert
            Assert.True(result.Count > 0);
        }

        [Theory]
        [InlineData("БББО-01-16 (КБ-1)10.03.01", "БББО-01-16")]
        [InlineData("БББО-02-16 (КБ-1)10.03.01", "БББО-02-16")]
        [InlineData("БББО-03-16 (КБ-1)10.03.01", "БББО-03-16")]
        public void ParseGroupName_StringGroupName_ShouldBeReturnParseGroup(string groupName, string expected)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            //Act
            var result = importDataExcelService.ParseGroupName(groupName);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("I", false)]
        [InlineData("II", true)]
        [InlineData("III", false)]
        [InlineData("", false)]
        public void ParseParityWeek_BoolParity_ShouldBeReturnTrueOrFalse(string parity, bool expected)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            //Act
            var result = importDataExcelService.ParseParityWeek(parity);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("пр", CourseType.Practicte)]
        [InlineData("лр", CourseType.LaboratoryWork)]
        [InlineData("лб", CourseType.LaboratoryWork)]
        [InlineData("лек", CourseType.Lecture)]
        [InlineData("зач", CourseType.ControlCourse)]
        [InlineData("консультация", CourseType.СonsultationCourse)]
        [InlineData("экзамен", CourseType.ExamCourse)]
        [InlineData("лк", CourseType.Lecture)]
        public void ParseCourseType_CourseType_ShouldBeReturnValueCourseType(string courseType, CourseType expected)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            /* case "пр":
              return CourseType.Practicte;
              case "лр":
              return CourseType.LaboratoryWork;
              case "лб":
              return CourseType.LaboratoryWork;
              case "лек":
              return CourseType.Lecture;
              case "зач":
              return CourseType.ControlCourse;
              case "консультация":
              return CourseType.СonsultationCourse;
              case "экзамен":
              return CourseType.ExamCourse;
              case "лк":
              return CourseType.Lecture;
              */

            //Act
            var result = importDataExcelService.ParseCourseType(courseType);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,2,3 н Предмет", 1)]
        [InlineData("1 н Предмет", 2)]
        [InlineData("Предмет", 3)]
        public void ParseNumberWeek_ListInt_ShouldBeReturnListInt(string courseName, int flag)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);
            var expected = GetValueByFlag(flag);

            //Act
            var result = importDataExcelService.ParseNumberWeek(courseName);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1 2 3 н Предмет", true)]
        [InlineData("1 н Предмет", true)]
        [InlineData("Предмет", false)]
        public void IsNumberContains_Bool_ShouldBeReturnTrueOrFalse(string stringValue, bool expected)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            //Act
            var result = importDataExcelService.IsNumberContains(stringValue);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1 2 3 н Предмет", "Предмет")]
        [InlineData("Предмет", "Предмет")]
        [InlineData("", "")]
        public void PrepareCourseName_String_ShouldBeReturnStringWithoutNumbers(string stringValue, string expected)
        {
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);

            //Act
            var result = importDataExcelService.PrepareCourseName(stringValue);

            //Assert
            Assert.Equal(expected, result);
        }


        #region MyRegion

        public List<int> GetValueByFlag(int flag)
        {
            switch (flag)
            {
                case 1:
                    return new List<int> {1, 2, 3};
                case 2:
                    return new List<int> {1};
                case 3:
                    return new List<int>();

                default:
                    return new List<int>();
            }
        }

        #endregion
    }
}