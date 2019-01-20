using System;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek
{
    /// <summary>
    /// Модель для запросов. (!)
    /// </summary>
    public class ParityOfTheWeekRequestModel
    {
        /// <summary>
        /// Выбранная пользователем дата, по которой нужно вывести информацию.
        /// </summary>
        public DateTime SelectedDateTime { get; set; }
    }
}
