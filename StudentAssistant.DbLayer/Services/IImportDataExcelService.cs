using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для импорта данных из Excel.
    /// </summary>
    public interface IImportDataExcelService
    {
        /// <summary>
        /// Импортирует данные из Excel файла.
        /// </summary>
        List<ImportDataExcelModel> LoadExcelFile();

        /// <summary>
        /// Возвращает список с данными для расписания.
        /// </summary>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels();
    }
}
