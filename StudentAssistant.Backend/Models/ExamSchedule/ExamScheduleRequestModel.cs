using System.ComponentModel.DataAnnotations;

namespace StudentAssistant.Backend.Models.ExamSchedule
{
    /// <summary>
    /// Модель запроса для получения расписания экзаменационной сессии.
    /// </summary>
    public class ExamScheduleRequestModel
    {
        /// <summary>
        /// Название учебной группы.
        /// </summary>
        [Required]
        public string GroupName { get; set; }
    }
}