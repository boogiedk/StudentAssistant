using System.Collections.Generic;

namespace StudentAssistant.Backend.Models.CourseSchedule.ViewModels
{
    /// <summary>
    /// Модель представления для расписания.
    /// </summary>
    public class CourseScheduleViewModel
    {
        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

        /// <summary>
        /// Список с данными о днях из расписания.
        /// </summary>
        public List<CourseViewModel> CoursesViewModel { get; set; }
    }
}