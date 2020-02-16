using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.Backend.Models.Account.Responses;
using StudentAssistant.DbLayer;

namespace StudentAssistant.Backend.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Метод для получения профиля пользователя по его идентификатору.
        /// </summary>
        /// <param name="requestUserId"></param>
        /// <returns></returns>
       Task<ProfileViewModel> Get(string requestUserId);

        /// <summary>
        /// Метод для регистрации пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AccountRegisterResponse> Register(AccountRegisterRequest model, CancellationToken cancellationToken);
    }
}