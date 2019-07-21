namespace StudentAssistant.DbLayer.Models.CourseSchedule
{
    /// <summary>
    /// Модель для передачи параметров, по которым будет фильтроваться расписание.
    /// </summary>
    public class CourseScheduleParameters
    {
        /// <summary>
        /// Номер недели.
        /// </summary>
        public int NumberWeek { get; set; }

        /// <summary>
        /// Название дня недели.
        /// </summary>
        public string NameOfDayWeek { get; set; }

        /// <summary>
        /// Чётность недели.
        /// </summary>
        public bool ParityWeek { get; set; }
    }
}