using System;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek.ViewModels
{
    /// <summary>
    /// Модель для отображения данных о дне недели по заданному <see cref="DateTimeOffset"/> параметру.
    /// </summary>
    public class ParityOfTheWeekViewModel
    {
        /// <summary>
        /// Время создания запроса на получение данных.
        /// </summary>
        public string DateTimeRequest { get; set; }

        /// <summary>
        /// Хранит <see cref="string"/> true, если неделя чётная, иначе <see cref="string"/> false.
        /// </summary>
        public string ParityOfWeekToday { get; set; }

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
        /// Текстовый статус дня: учебный, выходной, каникулы, сессия.
        /// </summary>
        public string StatusDay { get; set; }

        /// <summary>
        /// Хранит <see cref="bool"/> true, если неделя чётная, иначе<see cref="bool"/> false.
        /// </summary>
        public bool IsParity { get; set; }

        /// <summary>
        /// Хранит <see cref="string"/> с значением "Сегодня" или "Выбрано".
        /// </summary>
        public string SelectedDateStringValue { get; set; }

        public override string ToString()
        {
            return $"DateTimeRequest: {DateTimeRequest} DayOfName: {DayOfName} NumberOfSemester: {NumberOfSemester} StatusDay: {StatusDay}";
        }
    }
}
