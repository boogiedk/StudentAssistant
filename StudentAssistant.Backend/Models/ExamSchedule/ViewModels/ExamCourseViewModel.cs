using System;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Models.ExamSchedule.ViewModels
{
    public class ExamCourseViewModel
    {
        public Guid Id { get; set; }
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
        /// Название группы.
        /// </summary>
        public StudyGroupModel StudyGroupModel { get; set; }

        /// <summary>
        /// Нач. Занятий.
        /// </summary>
        public string StartOfClasses { get; set; }
    }
}