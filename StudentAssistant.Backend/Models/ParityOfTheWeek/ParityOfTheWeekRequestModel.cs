using System;
using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek
{
    /// <summary>
    /// Модель запроса на получение данных о дне недели.
    /// </summary>
    public class ParityOfTheWeekRequestModel
    {
        /// <summary>
        /// Выбранная пользователем дата, по которой нужно получить информацию.
        /// </summary>
        [Required]
        public DateTimeOffset SelectedDateTime { get; set; }
    }
}
