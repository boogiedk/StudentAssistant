using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
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
            Assert.Equal(expectedDayOfWeek,result.Item2);
        }
    }
}