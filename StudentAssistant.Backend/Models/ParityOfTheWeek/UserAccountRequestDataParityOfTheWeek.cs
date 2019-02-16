using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek
{
    /// <summary>
    /// Модель, содержащая данные о пользователе, которая нужна для отобрадения корректной информации.
    /// </summary>
    public class UserAccountRequestDataParityOfTheWeek
    {
        /// <summary>
        /// Идентификатор часового пояса пользователя.
        /// </summary>
        public string TimeZoneId { get; set; }
    }
}
