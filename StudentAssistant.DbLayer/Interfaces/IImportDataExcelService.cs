using System.Collections.Generic;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Models.ImportData;
using StudentAssistant.DbLayer.Services.Implementation;

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

        /// <summary>
        /// Импортирует данные расписания экзаменов из Excel файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerable<ExamScheduleDatabaseModel> GetExamScheduleDatabaseModels(string fileName);
    }
}
