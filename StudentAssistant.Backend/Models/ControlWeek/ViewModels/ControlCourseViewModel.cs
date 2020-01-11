using System;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Models.ControlWeek.ViewModels
{
    /// <summary>
    /// Модель представления с данными о днях из расписания зачетной недели.
    /// </summary>
    public class ControlCourseViewModel
    {
        public ControlCourseViewModel()
        {
            CourseName = String.Empty;
            CourseType = String.Empty;
            CoursePlace = String.Empty;
        }
        
        public Guid Id { get; set; }
        
        /// <summary>
        /// Название предмета.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Номер предмета.
        /// </summary>
        public int CourseNumber { get; set; }
        
        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

        /// <summary>
        /// Тип предмета.
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// Полное имя преподавателя.
        /// </summary>
        public TeacherModel TeacherModel { get; set; }

        /// <summary>
        /// Место проведения.
        /// </summary>
        public string CoursePlace { get; set; }

        /// <summary>
        /// Нач. Занятий.
        /// </summary>
        public string StartOfClasses { get; set; }

        /// <summary>
        /// Окончание Занятий.
        /// </summary>
        public string EndOfClasses { get; set; }
        
        public StudyGroupModel StudyGroupModel { get; set; }
    }
}