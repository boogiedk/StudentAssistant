using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Models.Exam
{
    // Модель с параметрами для получения расписания экзаменов.
    public class ExamScheduleParametersModel
    {
        /// <summary>
        /// Тип предмета. (Экзамен)
        /// </summary>
        public CourseType CourseTypeExam { get; set; }
        
        /// <summary>
        /// Тип предмета. (Консультация)
        /// </summary>
        public CourseType CourseTypeConsultation { get; set; }
        
        /// <summary>
        /// Группа.
        /// </summary>
        public StudyGroupModel StudyGroupModel { get; set; }
    }
}