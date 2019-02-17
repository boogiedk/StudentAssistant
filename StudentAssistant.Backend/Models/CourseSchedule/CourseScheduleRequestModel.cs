using System;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель запросов на получение расписания.
    /// </summary>
    public class CourseScheduleRequestModel
    {
        /// <summary>
        /// Время, по которому нужно вернуть расписание.
        /// </summary>
        public DateTimeOffset DateTimeRequest { get; set; }
    }
}
