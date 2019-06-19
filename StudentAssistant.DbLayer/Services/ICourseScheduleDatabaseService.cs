using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для работы с базой данных.
    /// </summary>
    public interface ICourseScheduleDatabaseService
    {
        /// <summary>
        /// Обновить расписание в базе данных.
        /// </summary>
        /// <param name="courseScheduleDatabaseModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(List<CourseScheduleDatabaseModel> courseScheduleDatabaseModel, CancellationToken cancellationToken);
    }
}
