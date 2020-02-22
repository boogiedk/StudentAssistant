using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Services.Implementation;
using StudentAssistant.Tests.Helpers;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    public class CourseScheduleFileServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ImportDataExcelService>> _logger;
        private readonly IImportDataExcelService _importDataExcelService;

        public CourseScheduleFileServiceTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _mockMapper = fixture.Freeze<Mock<IMapper>>();
            _logger = fixture.Freeze<Mock<ILogger<ImportDataExcelService>>>();
            _importDataExcelService = new ImportDataExcelService(_logger.Object);
        }

        [Theory]
        [InlineData(5, 216)]
        public async void GetFromExcelFile_CountList_ShouldBeReturnListFromExcel(int fileNameFlag, int expected)
        {
            // Arange
            var fileName = TestValueProvider.GetValueStringByFlag(fileNameFlag);
            var courseScheduleFileService = new CourseScheduleFileService(_importDataExcelService);

            // Act
            var result = await courseScheduleFileService.GetFromExcelFile(fileName);

            // Assert
            Assert.Equal(result.Count, expected);
        }

        [Theory]
        [InlineData(1, 60)]
        public async void GetExamScheduleFromExcelFile_CountList_ShouldBeReturnListFromExcel(int fileNameFlag,
            int expected)
        {
            // Arange
            var fileName = TestValueProvider.GetValueStringByFlag(fileNameFlag);
            var courseScheduleFileService = new CourseScheduleFileService(_importDataExcelService);

            // Act
            var result = await courseScheduleFileService.GetExamScheduleFromExcelFile(fileName);

            // Assert
            Assert.Equal(result.Count, expected);
        }
    }
}