using System;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.DownloadFileService;

namespace StudentAssistant.Backend.Interfaces
{
    /// <summary>
    /// Сервис для скачивания Excel файла.
    /// </summary>
   public interface IFileService
   {
       /// <summary>
       /// Проверяет актуальность Excel файла.
       /// </summary>
       /// <param name="datetimeUtc"></param>
       /// <param name="fileName"></param>
       /// <returns></returns>
       Task<bool> CheckExcelFile(DateTime datetimeUtc,string fileName);

       /// <summary>
       /// Скачивает Excel файл с URL университета.
       /// </summary>
       /// <returns></returns>
       Task DownloadAsync(
           DownloadFileParametersModel downloadFileParametersModel,
           CancellationToken cancellationToken);

       /// <summary>
       /// Скачивает Excel файл с URL университета.
       /// </summary>
       /// <returns></returns>
       Task DownloadByLinkAsync(
           Uri uri, string fileName,
           CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает дату последней записи в файл.
        /// </summary>
        /// <returns></returns>
        Task<DateTime> GetLastWriteTime(string fileName);
   }
}
