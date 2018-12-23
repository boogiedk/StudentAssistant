using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
