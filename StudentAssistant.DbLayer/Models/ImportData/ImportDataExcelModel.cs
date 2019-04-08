namespace StudentAssistant.DbLayer.Models.ImportData
{
    /// <summary>
    /// Модель для импорта данных из Excel.
    /// </summary>
    public class ImportDataExcelModel
    {
        /// <summary>
        /// Номер пары.
        /// </summary>
        public double CourseNumber { get; set; }

        /// <summary>
        /// Нач. Занятий.
        /// </summary>
        public string StartOfClasses { get; set; }

        /// <summary>
        /// Окончание Занятий.
        /// </summary>
        public string EndOfClasses { get; set; }

        /// <summary>
        /// Группа.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// День недели.
        /// </summary>
        public string DayOfTheWeek { get; set; }

        /// <summary>
        /// Неопределенная ячейка.
        /// </summary>
        public double OtherCell { get; set; }

        /// <summary>
        /// Предмет.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Неделя.
        /// </summary>
        public string ParityWeek { get; set; }

        /// <summary>
        /// Вид занятий.
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// ФИО преподавателя.
        /// </summary>
        public string TeacherFullName { get; set; }

        /// <summary>
        /// Номер аудитории.
        /// </summary>
        public string CoursePlace { get; set; }
    }
}
