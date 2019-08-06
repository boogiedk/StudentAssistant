using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.DownloadFileService;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class DownloadFileService : IDownloadFileService
    {
        // вынести в конфиг
        private readonly string _pathToFile = Path.Combine("Infrastructure", "ScheduleFile");
        private readonly string _localFileName = "scheduleFile.xlsx";
        //
        public Task<bool> CheckCurrentExcelFile(DateTimeOffset dateTimeOffset) => Task.Run(() =>
        {
            var lastAccessTimeUtc = File.GetLastAccessTimeUtc(
                Path.Combine($"{_pathToFile}, {_localFileName}"));

            return lastAccessTimeUtc.Date == dateTimeOffset.Date;
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
    }
}
