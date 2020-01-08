using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Models.ImportData
{
    /// <summary>
    /// ООП модель "базы данных" с данными о расписании экзаменов.
    /// </summary>
    public class ExamScheduleDatabaseModel
    {
        /// <summary>
        /// Название предмета.
        /// </summary>
        public string CourseName { get; set; }
        
        /// <summary>
        /// Месяц проведения экзамена.
        /// </summary>
        public string Month { get; set; }
        
        /// <summary>
        /// Число проведения экзамена.
        /// </summary>
        public string NumberDate { get; set; }
        
        /// <summary>
        /// День недели проведения экзамена.
        /// </summary>
        public string DayOfWeek { get; set; }

        /// <summary>
        /// Тип предмета.
        /// </summary>
        public CourseType CourseType { get; set; }

        /// <summary>
        /// Полное имя преподавателя.
        /// </summary>
        public string TeacherFullName { get; set; }

        /// <summary>
        /// Место проведения.
        /// </summary>
        public string CoursePlace { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Нач. Занятий.
        /// </summary>
        public string StartOfClasses { get; set; }
    }
}