﻿using System.Collections.Generic;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services
{
    /// <summary>
    /// Сервис для импорта данных из Json.
    /// </summary>
    public interface IImportDataJsonService
    {
        /// <summary>
        /// Импортирует данные из json файла.
        /// </summary>
        /// <returns></returns>
        List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels();
    }
}