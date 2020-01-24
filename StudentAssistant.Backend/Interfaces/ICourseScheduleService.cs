using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.UpdateAsync;
using StudentAssistant.Backend.Services.Implementation;

namespace StudentAssistant.Backend.Interfaces
{
    /// <summary>
    /// Сервис для работы с расписанием.
    /// </summary>
    public interface ICourseScheduleService
    {
        /// <summary>
        /// Возвращает расписание по указанным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       Task<CourseScheduleViewModel> Get(CourseScheduleDtoModel input);

        /// <summary>
        /// Отправляет запрос на обновление расписания в базе данных.
        /// </summary>
        /// <returns></returns>
        Task<DownloadAsyncResponseModel> DownloadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает дату последнего изменения файла с расписанием.
        /// </summary>
        /// <returns></returns>
        Task<CourseScheduleUpdateResponseModel> GetLastAccessTimeUtc();

        /// <summary>
        /// Обновить расписание по ссылке.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadByLinkAsync(
            CourseScheduleUpdateByLinkAsyncModel request,
            CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные о расписании в базе данных.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateAsyncResponseModel> UpdateAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Обновляет данные в базе данных. (временно)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InsertAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Обновляет данные в базе данных. (временно)
        /// </summary>
        /// <returns></returns>
        void MarkLikeDeleted();
    }
}
