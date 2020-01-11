using System;
using System.Collections.Generic;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель с учебным расписанием.
    /// </summary>
    public class CourseScheduleModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Номер недели.
        /// </summary>
        public List<int> NumberWeek { get; set; }

        /// <summary>
        /// Чётность недели.
        /// </summary>
        public bool ParityWeek { get; set; }

        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

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
        public CourseType CourseType { get; set; }

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
        
        /// <summary>
        /// Группы, с которыми объединенные пары.
        /// </summary>
        public List<StudyGroupModel> CombinedGroup { get; set; }
    }
}