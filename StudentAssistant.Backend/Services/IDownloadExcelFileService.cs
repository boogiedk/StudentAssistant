using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Сервис для скачивания Excel файла.
    /// </summary>
   public interface IDownloadExcelFileService
   {
       /// <summary>
       /// Проверяет актуальность Excel файла.
       /// </summary>
       /// <param name="dateTimeOffset"></param>
       /// <returns></returns>
       Task<bool> CheckCurrentExcelFile(DateTimeOffset dateTimeOffset);

       /// <summary>
       /// Скачивает Excel файл с URL университета.
       /// </summary>
       /// <returns></returns>
       Task DownloadAsync(CancellationToken cancellationToken);
   }
}
