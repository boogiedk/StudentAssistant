using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
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
        /// Другое.
        /// </summary>
        [Description("Другое")]
        Other = 0,
    }
}
