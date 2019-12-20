using System.Collections.Generic;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

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
    }
}
