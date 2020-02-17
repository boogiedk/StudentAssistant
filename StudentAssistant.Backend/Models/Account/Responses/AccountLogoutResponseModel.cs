using Microsoft.AspNetCore.Identity;

namespace StudentAssistant.Backend.Models.Account.Responses
{
    /// <summary>
    /// Модель ответа.
    /// </summary>
    public class AccountLogoutResponseModel
    {
        /// <summary>
        /// Состояние операции.
        /// </summary>
        public IdentityResult IdentityResult { get; set; }
    }
}