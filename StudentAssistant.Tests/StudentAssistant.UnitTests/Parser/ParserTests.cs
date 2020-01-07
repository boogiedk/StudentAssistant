using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
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
            // Arrange
            var importDataExcelService = new ImportDataExcelService(_logger.Object);
            var fileName = @"C:\Users\ganz1\Desktop\VS Projects\StudentAssistant\StudentAssistant.Backend\Infrastructure\ScheduleFile\ekz_KBiSP_4-kurs_zima.xls";
            
            //Act
            var result = importDataExcelService.GetExamScheduleDatabaseModels(fileName).ToList();
            
            //Assert
            Assert.True(result.Count>0);
        }
    }
}