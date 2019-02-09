﻿using System.Collections.Generic;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class CoursesViewModel
    {
        /// <summary>
        /// Номер недели.
        /// </summary>
        public List<int> NumberWeek { get; set; }

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
    }
}