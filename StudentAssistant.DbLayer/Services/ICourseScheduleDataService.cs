using System.Collections.Generic;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для работы с базой данных и сервисом для расписания.
    /// </summary>
    public interface ICourseScheduleDataService
    {
        /// <summary>
        /// Возвращает расписание, взятое из Json файла.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleFromJsonFile(CourseScheduleParameters input);

        /// <summary>
        /// Возвращает расписание, взятое из Excel файла.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleFromExcelFile(CourseScheduleParameters input);
    }
}
