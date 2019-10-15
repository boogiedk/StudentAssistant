using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с базой данных в MongoDb. 
    /// </summary>
    public interface ICourseScheduleMongoDbService
    {
        /// <summary>
        /// Добавляет документы с расписанием.
        /// </summary>
        /// <param name="input"></param>
        void InsertAsync(List<CourseScheduleDatabaseModel> input);

        /// <summary>
        /// Удаляет все документы с расписанием.
        /// </summary>
        void RemoveAllAsync();

        /// <summary>
        /// Возвращает документы с расписанием по указанными параметрам.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<CourseScheduleDatabaseModel>> GetByParameters(CourseScheduleParameters parameters);
    }
}