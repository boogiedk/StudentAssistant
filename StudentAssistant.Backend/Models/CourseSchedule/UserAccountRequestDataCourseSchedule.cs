namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель, содержащая данные о пользователе, которая нужна для отобрадения корректной информации.
    /// </summary>
    public class UserAccountRequestDataCourseSchedule
    {
        /// <summary>
        /// Идентификатор часового пояса пользователя.
        /// </summary>
        public string TimeZoneId { get; set; }
    }
}