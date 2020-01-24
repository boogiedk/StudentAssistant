using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.Exam;

namespace StudentAssistant.DbLayer.Interfaces
{
    public interface IExamScheduleDatabaseService
    {
        /// <summary>
        /// Добавляет данные с расписанием экзаменов.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        Task InsertAsync(List<ExamScheduleDatabaseModel> input, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает данные с расписанием экзаменов по указанными параметрам.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<ExamScheduleDatabaseModel>> GetByParameters(ExamScheduleParametersModel parameters);

        /// <summary>
        /// Обновляет расписание экзаменов в базе данных.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(List<ExamScheduleDatabaseModel> input, CancellationToken cancellationToken);

        /// <summary>
        /// Помечает записи в базе данных как удаленные.
        /// </summary>
        void MarkLikeDeleted();
    }
}