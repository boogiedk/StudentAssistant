
using System.ComponentModel;

namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
    /// <summary>
    /// Тип предмета.
    /// </summary>
    public enum CourseType
    {
        /// <summary>
        /// Практика.
        /// </summary>
        [Description("Практика")]
        Practicte = 1,

        /// <summary>
        /// Лекция.
        /// </summary>
        [Description("Лекция")]
        Lecture = 2,

        /// <summary>
        /// Лабораторная.
        /// </summary>
        [Description("Лабораторная")]
        LaboratoryWork = 3,
        
        /// <summary>
        /// Зачёт.
        /// </summary>
        [Description("Зачёт")]
        ControlCourse = 4,
        
        /// <summary>
        /// Экзамен.
        /// </summary>
        [Description("Экзамен")]
        ExamCourse = 5,
        
        /// <summary>
        /// Консультация.
        /// </summary>
        [Description("Консультация")]
        СonsultationCourse = 6,

        /// <summary>
        /// Другое.
        /// </summary>
        [Description("Другое")]
        Other = 0,
    }
}
