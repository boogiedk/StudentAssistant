using System.Collections.Generic;

namespace StudentAssistant.Backend.Models.CourseSchedule.ViewModels
{
    /// <summary>
    /// Модель представления для расписания.
    /// </summary>
    public class CourseScheduleViewModel
    {
        public CourseScheduleViewModel()
        {
            CoursesViewModel = new List<CourseViewModel>();
        }

        /// <summary>
        /// Время запроса.
        /// </summary>
        public string DatetimeRequest { get; set; }

        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

        /// <summary>
        /// Дата последнего изменения файла с расписанием.
        /// </summary>
        public string UpdateDatetime { get; set; }

        /// <summary>
        /// Список с данными о днях из расписания.
        /// </summary>
        public List<CourseViewModel> CoursesViewModel { get; set; }
    }
}