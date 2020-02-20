using System;
using System.IO;
using System.Security.Policy;
using System.Threading;
using StudentAssistant.Backend.Models.DownloadFileService;
using StudentAssistant.Backend.Services.Implementation;
using Xunit;

namespace StudentAssistant.Tests.StudentAssistant.UnitTests.Backend
{
    /// <summary>
    /// Принцип именования методов:
    /// [Тестируемый метод]_[Возвращаемое значение]_[Ожидаемое поведение]
    /// </summary>
    public class FileServiceTests
    {
        [Theory]
        [InlineData("02-05-2020", 1, false)]
        [InlineData("01-26-2020", 1, false)]
        [InlineData("07-05-2019", 1, false)]
        public async void CheckExcelFile_Bool_ShouldBeReturnFalse(DateTime datetimeUtc, int flag, bool expected)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = GetFilePath(flag);

            // Act
            var result = await fileService.CheckExcelFile(datetimeUtc, fileName);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, "01-31-2020", true)]
        public async void CheckExcelFile_Bool_ShouldBeReturnTrue(int flag, DateTime datetimeUtc, bool expected)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = GetFilePath(flag);

            // Act
            var result = await fileService.CheckExcelFile(datetimeUtc, fileName);

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
            var fileName = GetFilePath(flag);

            // Act
            var result = await fileService.GetLastWriteTime(fileName);

            // Assert
            Assert.NotEqual(expected.Date, result.Date);
        }

        [Theory]
        [InlineData(1, "01-31-2020")]
        public async void GetLastWriteTime_DateTime_ShouldBeReturnTrueLastAccessTimeUtc(int flag, DateTime expected)
        {
            // Arrange
            var fileService = new FileService();
            var fileName = GetFilePath(flag);

            // Act
            var result = await fileService.GetLastWriteTime(fileName);

            // Assert
            Assert.Equal(expected.Date, result.Date);
        }

        [Theory]
        [InlineData(2, 3)]
        public async void DownloadByLinkAsync_DoneTask_ShouldBeDownloadFileByLink(int urlFlag, int fileNameFlag)
        {
            // Arrange
            var fileService = new FileService();
            var url = GetFilePath(urlFlag);
            var fileName = Path.Combine(GetFilePath(fileNameFlag) + ".xlsx");

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
            var fileName = Path.Combine(GetFilePath(fileNameFlag));
            var downloadFileParametersModel = new DownloadFileParametersModel
            {
                //https://www.mirea.ru/upload/medialibrary/8b7/KBiSP-4-kurs-2-sem.xlsx
                PathToFile = string.Empty,
                RemoteUri = new Uri("https://www.mirea.ru/upload/medialibrary/8b7/"),
                FileNameLocal = fileName,
                FileNameRemote = "KBiSP-4-kurs-2-sem",
                FileFormat = "xlsx"
            };

            // Act
            await fileService.DownloadAsync(downloadFileParametersModel, CancellationToken.None);

            // Assert
            var result = File.Exists(fileName + ".xlsx");
            Assert.True(result);
        }


        #region Helpers

        private string GetFilePath(int flag)
        {
            switch (flag)
            {
                case 1:
                    return Path.Combine("TestFiles", "examScheduleFileTest.xls");
                case 2:
                    return @"https://www.mirea.ru/upload/medialibrary/8b7/KBiSP-4-kurs-2-sem.xlsx";
                case 3:
                    return Guid.NewGuid().ToString();
                case 4:
                    return Path.Combine("TestFiles");

                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}