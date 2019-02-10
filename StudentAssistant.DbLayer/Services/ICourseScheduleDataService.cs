using System.Collections.Generic;

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
