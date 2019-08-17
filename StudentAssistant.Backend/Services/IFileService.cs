using System;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.DownloadFileService;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Сервис для скачивания Excel файла.
    /// </summary>
   public interface IFileService
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
       Task DownloadAsync(
           DownloadFileParametersModel downloadFileParametersModel,
           CancellationToken cancellationToken);

       /// <summary>
       /// Возвращает дату последней записи в файл.
       /// </summary>
       /// <returns></returns>
       Task<DateTime> GetLastWriteTime();
   }
}
