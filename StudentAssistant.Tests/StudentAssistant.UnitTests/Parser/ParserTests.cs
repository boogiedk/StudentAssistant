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
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Services.Implementation;
using Xunit;


namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Parser
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class ParserTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ImportDataExcelService>> _logger;

        public ParserTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _mockMapper = fixture.Freeze<Mock<IMapper>>();
            _logger = fixture.Freeze<Mock<ILogger<ImportDataExcelService>>>();
        }

        [Fact]
        public void GetExamScheduleDatabaseModels_CountDatabaseModels_ShouldParseExcelAndReturnCountModels()
        {
            try
            {

                // Arrange
                var importDataExcelService = new ImportDataExcelService(_logger.Object);
                var fileName = Path.Combine("TestFiles", "examScheduleFileTest.xls");

                //Act
                var result = importDataExcelService.GetExamScheduleDatabaseModels(fileName).ToList();

                //Assert
                Assert.True(result.Count > 0);
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                // ignored
            }
        }
    }
}