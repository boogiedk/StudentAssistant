using System.ComponentModel;

namespace StudentAssistant.Backend.Models
{
    /// <summary>
    /// Тип статуса дня.
    /// </summary>
    public enum StatusDayType
    {
        [Description("Каникулы")]
        /// <summary>
        /// Каникулы.
        /// </summary>
        Holiday = 1,

        [Description("Учебный день")]
        /// <summary>
        /// Учебный день.
        /// </summary>
        SchoolDay = 2,

        [Description("Сессия")]
        /// <summary>
        /// Экзамены. (Сессия)
        /// </summary>
        ExamsTime = 3,

        [Description("Выходной день")]
        /// <summary>
        /// Выходной день
        /// </summary>
        DayOff = 4,
    }
}
