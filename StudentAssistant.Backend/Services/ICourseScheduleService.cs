using StudentAssistant.Backend.Models.CourseSchedule;
using System.Collections.Generic;
using StudentAssistant.Backend.Models.CourseSchedule.ViewModels;

namespace StudentAssistant.Backend.Services
{
    /// <summary>
    /// Сервис для работы с расписанием.
    /// </summary>
    public interface ICourseScheduleService
    {
        /// <summary>
        /// Возвращает расписание по указанным параметрам.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<CourseScheduleResultModel> GetCourseSchedule(CourseScheduleRequestModel input);

        /// <summary>
        /// Подготавливает модель представления.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CourseScheduleViewModel PrepareCourseScheduleViewModel(List<CourseScheduleResultModel> input);
    }
}
