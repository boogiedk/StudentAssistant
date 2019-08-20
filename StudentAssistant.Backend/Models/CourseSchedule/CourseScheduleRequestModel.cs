using System;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель запроса на получение расписания.
    /// </summary>
    public class CourseScheduleRequestModel
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
