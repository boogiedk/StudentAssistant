using System.Collections.Generic;

namespace StudentAssistant.Backend.Services.Implementation
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
        /// Список с данными о днях из расписании.
        /// </summary>
        public List<CoursesViewModel> CoursesViewModel { get;set;}
    }
}