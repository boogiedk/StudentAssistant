using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для работы с базой данных и сервисом для расписания.
    /// </summary>
    public interface ICourseScheduleFileService
    {
        /// <summary>
        /// Возвращает расписание, взятое из Json файла и отфильтрованное по заданным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleFromJsonFileByParameters(CourseScheduleParameters input);

        /// <summary>
        /// Возвращает расписание, взятое из Excel файла и отфильтрованное по заданным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleFromExcelFileByParameters(CourseScheduleParameters input);

        /// <summary>
        /// Возвращает все расписание из Excel файла.
        /// </summary>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetFromExcel();
    }
}
