using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для работы с базой данных и сервисом для расписания.
    /// </summary>
    public interface ICourseScheduleDataService
    {
        /// <summary>
        /// Возвращает расписание, взятое из базы данных.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleFromDatabase(CourseScheduleParameters input);
    }
}
