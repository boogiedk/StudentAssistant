using System.Collections.Generic;
using StudentAssistant.Backend.Models.CourseSchedule;

namespace StudentAssistant.Backend.Models.ExamSchedule.ViewModels
{
    public class ExamScheduleViewModel
    {
        public List<ExamCourseViewModel> ExamCourseViewModel { get; set; }
        public string DatetimeRequest { get; set; }
        public string UpdateDatetime { get; set; }
    }
}