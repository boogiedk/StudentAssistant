using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Moq;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using Xunit;
using StudentAssistant.Parser;


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

        public ParserTests()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            _mapper = new MapperConfiguration(c =>
                c.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _mockMapper = fixture.Freeze<Mock<IMapper>>();
        }

        [Fact]
        public void ExcelParser_DataFromExcelFile_ParseExcelFile()
        {
            // Arrange
            var expected = "Test Value from Parser";
            
            // Act
            var result = ParserUnitTest.GetTestValue;

            // Assert
            Assert.Equal(expected,result);
        }
    }
}