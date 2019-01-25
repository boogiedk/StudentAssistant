using System.ComponentModel;

namespace StudentAssistant.Backend.Models.ParityOfTheWeek
{
    /// <summary>
    /// Тип статуса дня.
    /// </summary>
    public enum StatusDayType
    {
        /// <summary>
        /// Каникулы.
        /// </summary>
        [Description("Каникулы")]
        Holiday = 1,

        /// <summary>
        /// Учебный день.
        /// </summary>
        [Description("Учебный день")]
        SchoolDay = 2,

        /// <summary>
        /// Экзамены. (Сессия)
        /// </summary>
        [Description("Сессия")]
        ExamsTime = 3,

        /// <summary>
        /// Выходной день
        /// </summary>
        [Description("Выходной день")]
        DayOff = 4,
    }
}
