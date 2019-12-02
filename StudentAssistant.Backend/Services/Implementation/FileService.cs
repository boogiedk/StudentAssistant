using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.DownloadFileService;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class FileService : IFileService
    {
        // вынести в конфиг
        private readonly string _pathToFile = Path.Combine("Infrastructure", "ScheduleFile", "scheduleFile.xlsx");
        //

        public Task<bool> CheckExcelFile(DateTime datetimeUfc) => Task.Run(() =>
        {
            var lastAccessTimeUtc = File.GetLastAccessTimeUtc(
                Path.Combine($"{_pathToFile}"));

            return lastAccessTimeUtc.Date == datetimeUfc.Date;
        });

        public async Task DownloadAsync(
            DownloadFileParametersModel downloadFileParametersModel,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var client = new HttpClient())
                {
                    using (var result = await client.GetAsync(
                        Path.Combine(
                            $"{downloadFileParametersModel.RemoteUri}" +
                            $"{downloadFileParametersModel.FileNameRemote}.{downloadFileParametersModel.FileFormat}"
                        ),
                        cancellationToken))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            var fileBytes = await result.Content.ReadAsByteArrayAsync();

                            await File.WriteAllBytesAsync(
                                Path.Combine(
                                    $"{downloadFileParametersModel.PathToFile}",
                                    $"{downloadFileParametersModel.FileNameLocal}.{downloadFileParametersModel.FileFormat}"),
                                fileBytes, cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public async Task DownloadByLinkAsync(Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var client = new HttpClient())
                {
                    using (var result = await client.GetAsync(uri,
                        cancellationToken))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            var fileBytes = await result.Content.ReadAsByteArrayAsync();

                            await File.WriteAllBytesAsync(
                             _pathToFile,
                                fileBytes, cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public Task<DateTime> GetLastWriteTime() => Task.Run(() =>
        {
            var lastAccessTimeUtc = File.GetLastWriteTime (
                Path.Combine($"{_pathToFile}"));

            return lastAccessTimeUtc;
        });
    }
}
