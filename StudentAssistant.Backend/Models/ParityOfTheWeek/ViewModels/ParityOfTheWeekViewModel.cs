using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Models.ViewModels
{
    /// <summary>
    /// Модель для отображения данных с формированием данных о заданном <see cref="DateTime"/> параметре.
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
        /// Текстовый статус дня: учебный, выходной, каникулы, сессия.
        /// </summary>
        public string StatusDay { get; set; }
    }
}
