using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Interfaces
{
    /// <summary>
    /// Сервис для импорта данных из Excel.
    /// </summary>
    public interface IImportDataExcelService
    {
        /// <summary>
        /// Импортирует данные расписания из Excel файла.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels(string fileName);
    }
}
