using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для импорта данных из Excel.
    /// </summary>
    public interface IImportDataExcelService
    {
        /// <summary>
        /// Возвращает список с данными для расписания.
        /// </summary>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels();
    }
}
