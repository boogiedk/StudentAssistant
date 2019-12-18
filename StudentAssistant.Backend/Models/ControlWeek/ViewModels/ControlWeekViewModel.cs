using System.Collections.Generic;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;

namespace StudentAssistant.Backend.Models.ControlWeek.ViewModels
{
    /// <summary>
    /// Модель представления для расписания.
    /// </summary>
    public class ControlWeekViewModel
    {
        public ControlWeekViewModel()
        {
            ControlCourseViewModel = new List<ControlCourseViewModel>();
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
        public List<ControlCourseViewModel> ControlCourseViewModel { get; set; }
        
        
        public override string ToString()
        {
            return $"DatetimeRequest: {DatetimeRequest} NameOfDayWeek: {NameOfDayWeek} CoursesViewModel: {ControlCourseViewModel.Count}";
        }
        
    }
}