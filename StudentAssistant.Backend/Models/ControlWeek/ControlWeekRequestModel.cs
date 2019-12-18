using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.ControlWeek
{
    /// <summary>
    /// Модель запроса для получения расписания зачетной недели.
    /// </summary>
    public class ControlWeekRequestModel
    {
        /// <summary>
        /// Название учебной группы.
        /// </summary>
        [Required]
        public string GroupName { get; set; }
    }
}