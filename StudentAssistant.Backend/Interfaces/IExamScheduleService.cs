using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.DownloadAsync;
using StudentAssistant.Backend.Models.ExamSchedule;
using StudentAssistant.Backend.Models.ExamSchedule.ViewModels;

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
    }
}