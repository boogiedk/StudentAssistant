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
        /// Возвращает расписание, взятое из Excel файла и отфильтрованное по заданным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       Task<IEnumerable<CourseScheduleDatabaseModel>> GetFromExcelFileByParameters(CourseScheduleParameters input);
    }
}
