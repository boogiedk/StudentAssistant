using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MongoDB.Bson;

namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
    /// <summary>
    /// Модель с учебным расписанием.
    /// </summary>
    public class CourseScheduleDatabaseModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Номер недели.
        /// </summary>
        [NotMapped]
        public List<int> NumberWeek { get; set; }

        /// <summary>
        /// Номер недели в виде строки.
        /// </summary>
        public string NumberWeekString { get; set; }

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
        /// Название группы.
        /// </summary>
        public StudyGroupModel StudyGroupModel { get; set; }

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
        [NotMapped]
        public List<StudyGroupModel> CombinedGroup { get; set; }

        /// <summary>
        /// Удалена ли запись.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Время создания записи.
        /// </summary>
        public DateTimeOffset DateTimeCreate { get; set; }

        /// <summary>
        /// Время обновления записи.
        /// </summary>
        public DateTimeOffset DateTimeUpdate { get; set; }

        /// <summary>
        /// Версия записи.
        /// </summary>
        public string Version { get; set; }


        public CourseScheduleDatabaseModel()
        {
            CombinedGroup = new List<StudyGroupModel>();
        }
    }
}