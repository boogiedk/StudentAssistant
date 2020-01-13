using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Interfaces
{
    public interface ICourseScheduleDatabaseService
    {
        /// <summary>
        /// Добавляет документы с расписанием.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
         Task InsertAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет все документы с расписанием.
        /// </summary>
        void RemoveAllAsync();

        /// <summary>
        /// Возвращает документы с расписанием по указанными параметрам.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<CourseScheduleDatabaseModel>> GetByParameters(CourseScheduleParameters parameters);

        /// <summary>
        /// Обновить расписание в базе данных.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken);
    }
}