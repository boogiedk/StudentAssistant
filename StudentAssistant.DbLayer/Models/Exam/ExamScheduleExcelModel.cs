using System;

namespace StudentAssistant.DbLayer.Models.Exam
{
    /// <summary>
    /// ООП модель Excel таблицы с расписанием экзамена.
    /// </summary>
    public class ExamScheduleExcelModel
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// Месяц проведения экзамена.
        /// </summary>
        public string Month { get; set; }
        
        /// <summary>
        /// Число проведения экзамена.
        /// </summary>
        public string Date { get; set; }
        
        /// <summary>
        /// Имя группы.
        /// </summary>
        public string GroupName { get; set; }
        
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
        /// Тип мероприятия (консультация/экзамен)
        /// </summary>
        public string CourseType { get; set; }
        
        /// <summary>
        /// Название предмета.
        /// </summary>
        public string CourseName { get; set; }
    }
}