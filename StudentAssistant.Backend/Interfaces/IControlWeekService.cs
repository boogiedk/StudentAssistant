using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.ControlWeek;
using StudentAssistant.Backend.Models.ControlWeek.ViewModels;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.UpdateAsync;
using StudentAssistant.Backend.Services.Implementation;

namespace StudentAssistant.Backend.Interfaces
{
    public interface IControlWeekService
    {
        /// <summary>
        /// Получить расписание зачетной недели.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ControlWeekViewModel> Get(ControlWeekRequestModel requestModel, CancellationToken cancellationToken);

        /// <summary>
        /// Скачать свежее расписание.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DownloadAsyncResponseModel> DownloadAsync(CancellationToken cancellationToken);


        /// <summary>
        /// Обновляет данные в базе данных.
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