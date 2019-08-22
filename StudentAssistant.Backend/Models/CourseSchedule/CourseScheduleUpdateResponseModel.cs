namespace StudentAssistant.Backend.Models.CourseSchedule
{
    /// <summary>
    /// Модель с датой последнего изменения файла с расписанием.
    /// </summary>
    public class CourseScheduleUpdateResponseModel
    {
        /// <summary>
        /// Дата последнего изменения файла с расписанием.
        /// </summary>
        public string UpdateDatetime { get; set; }
    }
}