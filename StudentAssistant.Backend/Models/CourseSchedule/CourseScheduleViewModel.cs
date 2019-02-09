using System.Collections.Generic;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CourseScheduleViewModel
    {
        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

        public List<CoursesViewModel> CoursesViewModel { get;set;}
    }
}