using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Models.Exam
{
    // Модель с параметрами для получения расписания экзаменов.
    public class ExamScheduleParametersModel
    {
        /// <summary>
        /// Тип предмета.
        /// </summary>
        public CourseType CourseType { get; set; }
        
        /// <summary>
        /// Группа.
        /// </summary>
        public StudyGroupModel StudyGroupModel { get; set; }
    }
}