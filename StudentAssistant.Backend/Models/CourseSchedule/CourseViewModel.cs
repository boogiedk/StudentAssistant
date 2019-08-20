using System.Collections.Generic;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель представления с данными о днях из расписания.
    /// </summary>
    public class CourseViewModel
    {
        public CourseViewModel()
        {
            CourseName = "Данных не найдено";
            TeacherFullName = "Данных не найдено";
            CourseType = "Данных не найдено";
            CoursePlace = "Данных не найдено";
            ParityWeek = "Данных не найдено";
        }


        /// <summary>
        /// Номер недели.
        /// </summary>
        public string NumberWeek { get; set; }

        /// <summary>
        /// Чётность недели.
        /// </summary>
        public string ParityWeek { get; set; }

        /// <summary>
        /// Название предмета.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Номер предмета.
        /// </summary>
        public int CourseNumber { get; set; }

        /// <summary>
        /// Тип предмета.
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// Полное имя преподавателя.
        /// </summary>
        public string TeacherFullName { get; set; }

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
    }
}