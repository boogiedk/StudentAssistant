﻿using System;
using System.ComponentModel;
using Humanizer;

namespace StudentAssistant.Backend.Models
{
    /// <summary>
    /// Модель для работы с сервисом, связанного с формированием данных о заданном <see cref="DateTime"/> параметре.
    /// </summary>
    public class ParityOfTheWeekModel
    {
        /// <summary>
        /// Время создания запроса на получение данных.
        /// </summary>
        public DateTime DateTimeRequest { get; set; }

        /// <summary>
        /// Хранит <see cref="bool"/> true, если неделя чётная, иначе <see cref="bool"/> false.
        /// </summary>
        public bool ParityOfWeekToday { get; set; }

        /// <summary>
        /// Хранит количество прошедших недель с сентября до <see cref="DateTime"/> переданного параметра.
        /// </summary>
        public int ParityOfWeekCount { get; set; }

        /// <summary>
        /// Номер части семестра.
        /// </summary>
        public int PartOfSemester { get; set; }

        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string DayOfName { get; set; }

        /// <summary>
        /// Номер семестра.
        /// </summary>
        public int NumberOfSemester { get; set; }

        /// <summary>
        /// Тип статуса дня: учебный, выходной, каникулы, сессия.
        /// </summary>
        public StatusDayType StatusDay { get; set; }
    }

    /// <summary>
    /// Тип статуса дня.
    /// </summary>
    public enum StatusDayType
    {
        [Description("Каникулы")]
        /// <summary>
        /// Каникулы.
        /// </summary>
        Holiday = 1,

        [Description("Учебный день")]
        /// <summary>
        /// Учебный день.
        /// </summary>
        SchoolDay = 2,

        [Description("Сессия")]
        /// <summary>
        /// Экзамены. (Сессия)
        /// </summary>
        ExamsTime = 3,

        [Description("Выходной день")]
        /// <summary>
        /// Выходной день
        /// </summary>
        DayOff = 4,
    }
}
