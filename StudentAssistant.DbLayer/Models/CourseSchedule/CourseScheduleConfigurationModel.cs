using System.Collections.Generic;

namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
    /// <summary>
    /// Класс с данными из файла конфигурации.
    /// </summary>
    public class CourseScheduleDataServiceConfigurationModel
    {
        /// <summary>
        /// Список с расписанием из файла конфигурации.
        /// </summary>
        public List<CourseScheduleDatabaseModel> ListCourseScheduleDatabaseModel { get; set; }
    }
}