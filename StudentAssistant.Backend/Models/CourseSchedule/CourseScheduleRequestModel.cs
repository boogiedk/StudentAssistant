using System;
using System.ComponentModel.DataAnnotations;

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
        public DateTime DateTimeRequest { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        [Required]
        public string GroupName { get; set; }

        public override string ToString()
        {
            return $"DateTimeRequest: {DateTimeRequest}. GroupName: {GroupName}";
        }
    }
}
