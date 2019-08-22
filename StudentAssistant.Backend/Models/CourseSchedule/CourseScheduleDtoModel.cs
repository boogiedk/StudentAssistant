using System;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Dto модель с данными для получения расписания.
    /// </summary>
    public class CourseScheduleDtoModel
    {
        /// <summary>
        /// Время, для которого нужно вернуть расписание.
        /// </summary>
        public DateTimeOffset DateTimeRequest { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string GroupName { get; set; }
    }
}
