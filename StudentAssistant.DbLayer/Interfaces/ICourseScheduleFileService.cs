using System.Collections.Generic;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Interfaces
{
    /// <summary>
    /// Сервис для работы с базой данных и сервисом для расписания.
    /// </summary>
    public interface ICourseScheduleFileService
    {
        /// <summary>
        /// Вовзращает все раписание из Excel файла.
        /// </summary>
        /// <returns></returns>
        Task<List<CourseScheduleDatabaseModel>> GetFromExcelFile(string fileName);
        
        /// <summary>
        /// Возвращает расписание, взятое из Excel файла и отфильтрованное по заданным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       IEnumerable<CourseScheduleDatabaseModel> GetFromExcelFileByParameters(CourseScheduleParameters input);

        /// <summary>
        /// Возвращает расписание с экзаменами.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<List<ExamScheduleDatabaseModel>> GetExamScheduleFromExcelFile(string fileName);
    }
}
