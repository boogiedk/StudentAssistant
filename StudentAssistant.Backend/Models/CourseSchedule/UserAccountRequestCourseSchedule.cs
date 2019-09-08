namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель, содержащая данные о пользователе.
    /// </summary>
    public class UserAccountRequestDataCourseSchedule
    {
        /// <summary>
        /// Идентификатор часового пояса пользователя.
        /// </summary>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Группа.
        /// </summary>
        public string GroupName { get; set; }
    }
}