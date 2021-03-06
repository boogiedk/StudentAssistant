﻿using System;
using System.Collections.Generic;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Models.CourseSchedule.ViewModels
{
    /// <summary>
    /// Модель представления с данными о днях из расписания.
    /// </summary>
    public class CourseViewModel
    {
        public CourseViewModel()
        {
            CourseName = String.Empty;
            CourseType = String.Empty;
            CoursePlace = String.Empty;
            ParityWeek = String.Empty;
        }

        public Guid Id { get; set; }
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
        /// Преподаватель.
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
        
        /// <summary>
        /// Группы, с которыми объединенные пары.
        /// </summary>
        public string CombinedGroup { get; set; }
    }
}