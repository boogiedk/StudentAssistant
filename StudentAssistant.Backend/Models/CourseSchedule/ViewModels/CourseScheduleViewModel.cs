using System.Collections.Generic;
using StudentAssistant.Backend.Services.Implementation;

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
        /// Список с данными о днях из расписании.
        /// </summary>
        public List<CoursesViewModel> CoursesViewModel { get;set;}
    }
}