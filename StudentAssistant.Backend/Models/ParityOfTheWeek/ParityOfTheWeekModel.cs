using System;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek
{
    /// <summary>
    /// Модель для работы с сервисом, связанного с формированием данных о заданном <see cref="DateTimeOffset"/> параметре.
    /// </summary>
    public class ParityOfTheWeekModel
    {
        /// <summary>
        /// Время создания запроса на получение данных.
        /// </summary>
        public DateTimeOffset DateTimeRequest { get; set; }

        /// <summary>
        /// Хранит <see cref="bool"/> true, если неделя чётная, иначе <see cref="bool"/> false.
        /// </summary>
        public bool ParityOfWeekToday { get; set; }

        /// <summary>
        /// Хранит количество прошедших недель с сентября до <see cref="DateTimeOffset"/> переданного параметра.
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
}
