using System.Threading.Tasks;
using StudentAssistant.Backend.Models.LogProvider;

namespace StudentAssistant.Backend.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Возвращает модель с логами минимального уровня Trace (все логи).
        /// </summary>
        /// <returns></returns>
        Task<LogDtoResponseModel> Get();
        
        /// <summary>
        /// Возвращает модель с логами минимального уровня, указанного в параметре. (all, info)
        /// TODO: поменять стринг на енум
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<LogDtoResponseModel> GetByType(string type);
    }
}