using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public DateTime DateTimeRequest { get; set; }
    }
}
