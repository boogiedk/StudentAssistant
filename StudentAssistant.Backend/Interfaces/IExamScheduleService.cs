using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.ExamSchedule;
using StudentAssistant.Backend.Models.ExamSchedule.ViewModels;
using StudentAssistant.Backend.Models.UpdateAsync;

namespace StudentAssistant.Backend.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с сервисом получения расписания экзаменов.
    /// </summary>
    public interface IExamScheduleService
    {
        /// <summary>
        /// Получить расписание экзаменов.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<ExamScheduleViewModel> Get(ExamScheduleRequestModel requestModel);

        /// <summary>
        /// Скачать новый файл расписания экзаменов.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DownloadAsyncResponseModel> DownloadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Обновить расписание экзаменов в базе данных.
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