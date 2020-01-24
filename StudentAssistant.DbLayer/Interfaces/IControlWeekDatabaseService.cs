using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Interfaces
{
    public interface IControlWeekDatabaseService
    {
        /// <summary>
        /// Добавляет данные с расписанием.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        Task InsertAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает данные с расписанием.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<CourseScheduleDatabaseModel>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет расписание в базе данных.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken);

        /// <summary>
        /// Помечает записи в базе данных как удаленные.
        /// </summary>
        void MarkLikeDeleted();
    }
}