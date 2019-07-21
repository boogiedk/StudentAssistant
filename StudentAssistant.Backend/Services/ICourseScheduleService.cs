using StudentAssistant.Backend.Models.CourseSchedule;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;

namespace StudentAssistant.Backend.Services
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
        List<CourseScheduleResultModel> Get(CourseScheduleDtoModel input);

        /// <summary>
        /// Подготавливает модель представления.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CourseScheduleViewModel PrepareViewModel(List<CourseScheduleResultModel> input);

        /// <summary>
        /// Отправляет запрос на обновление расписания в базе данных.
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(CancellationToken cancellationToken);
    }
}
