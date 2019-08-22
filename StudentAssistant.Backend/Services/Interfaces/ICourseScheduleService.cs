using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.CourseSchedule;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;

namespace StudentAssistant.Backend.Services.Interfaces
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
       CourseScheduleViewModel Get(CourseScheduleDtoModel input);

        /// <summary>
        /// Отправляет запрос на обновление расписания в базе данных.
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает дату последнего изменения файла с расписанием.
        /// </summary>
        /// <returns></returns>
        Task<CourseScheduleUpdateResponseModel> GetLastAccessTimeUtc();
    }
}
