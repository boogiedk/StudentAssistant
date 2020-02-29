using System;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Threading;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class FileServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FileServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("02-05-2020", 1, false)]
        [InlineData("01-26-2020", 1, false)]
        [InlineData("07-05-2019", 1, false)]
        public async void CheckExcelFile_Bool_ShouldBeReturnFalse(DateTime datetimeUtc, int flag, bool expected)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = TestValueProvider.GetValueStringByFlag(flag);

            // Act
            var result = await fileService.CheckExcelFile(datetimeUtc, fileName);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2,3, true)]
        public async void CheckExcelFile_Bool_ShouldBeReturnTrue(int urlFlag, int fileNameFlag, bool expected)
        {
            // Arrange
            var dateTime = DateTime.Now;
            var fileService = new FileService();
            var fileName = TestValueProvider.GetValueStringByFlag(fileNameFlag);
            await fileService.DownloadByLinkAsync(new Uri(TestValueProvider.GetValueStringByFlag(urlFlag)), fileName, CancellationToken.None);

            // Act
            var result = await fileService.CheckExcelFile(dateTime, fileName);
              _testOutputHelper.WriteLine(result.ToString(CultureInfo.CurrentCulture));

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, "02-05-2020")]
        [InlineData(1, "01-26-2020")]
        [InlineData(1, "07-05-2019")]
        public async void GetLastWriteTime_DateTime_ShouldBeReturnFalseLastAccessTimeUtc(int flag, DateTime expected)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = TestValueProvider.GetValueStringByFlag(flag);

            // Act
            var result = await fileService.GetLastWriteTime(fileName);

            // Assert
            Assert.NotEqual(expected.Date, result.Date);
        }

        [Theory]
        [InlineData(2,3)]
        public async void GetLastWriteTime_DateTime_ShouldBeReturnTrueLastAccessTimeUtc(int urlFlag, int fileNameFlag)
        {
            // Arrange
            var dateTime = DateTime.Now;
            var fileService = new FileService();
            var fileName = TestValueProvider.GetValueStringByFlag(fileNameFlag);
            await fileService.DownloadByLinkAsync(new Uri(TestValueProvider.GetValueStringByFlag(urlFlag)), fileName, CancellationToken.None);
            
            // Act
            var result = await fileService.GetLastWriteTime(fileName);

            // Assert
            Assert.Equal(dateTime.Date, result.Date);
        }

        [Theory]
        [InlineData(2, 3)]
        public async void DownloadByLinkAsync_DoneTask_ShouldBeDownloadFileByLink(int urlFlag, int fileNameFlag)
        {
            // Arrange
            var fileService = new FileService();
            var url = TestValueProvider.GetValueStringByFlag(urlFlag);
            var fileName = Path.Combine(TestValueProvider.GetValueStringByFlag(fileNameFlag) + ".xlsx");

            // Act
            await fileService.DownloadByLinkAsync(new Uri(url), fileName, CancellationToken.None);

            // Assert
            var result = File.Exists(fileName);
            Assert.True(result);
        }

        [Theory]
        [InlineData(3)]
        public async void DownloadByLinkAsync_DoneTask_ShouldBeDownloadFile(int fileNameFlag)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = Path.Combine(TestValueProvider.GetValueStringByFlag(fileNameFlag));
            var downloadFileParametersModel = new DownloadFileParametersModel
            {
                //https://www.mirea.ru/upload/medialibrary/97d/KBiSP-4-kurs-2-sem-_1_.xlsx
                PathToFile = string.Empty,
                RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/97d/"),
                FileNameLocal = fileName,
                FileNameRemote = "KBiSP-4-kurs-2-sem",
                FileFormat = "xlsx"
            };

            var f = (5f - 5f) * -1;
            var f2 = (5f - 5f) / -5f;
            
            Console.WriteLine(f);

            // Act
            await fileService.DownloadAsync(downloadFileParametersModel, CancellationToken.None);

            // Assert
            var result = File.Exists(fileName + ".xlsx");
            Assert.True(result);
        }
    }
}