using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class DownloadExcelFileService : IDownloadExcelFileService
    {
        // вынести в конфиг
        private readonly string _pathToFile = @"Infrastructure\ScheduleFile";
        private readonly string _remoteUri = "https://www.mirea.ru/upload/medialibrary/3d4/";
        private readonly string _localFileName = "scheduleFile.xlsx";
        //
        public Task<bool> CheckCurrentExcelFile(DateTimeOffset dateTimeOffset) => Task.Run(() =>
        {
            var lastAccessTimeUtc = File.GetLastAccessTimeUtc($@"{_pathToFile}\{_localFileName}");

            return lastAccessTimeUtc.Date == dateTimeOffset.Date;
        });

        public async Task DownloadAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // В будущем значение будет зависеть от выбранной группы
                var fileNameRemote = "KBiSP-3-kurs-2-sem.xlsx"; 

                // DownloadAsync the Web resource and save it into the current filesystem folder.
                await new WebClient().DownloadFileTaskAsync(
                    new Uri(_remoteUri + fileNameRemote), 
                    $@"{_pathToFile}\{_localFileName}");
            }
            catch (Exception e)
            {
                // поправить
                throw new NotSupportedException(e.Message);
            }
        }
    }
}
