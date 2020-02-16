using Microsoft.AspNetCore.Identity;

namespace StudentAssistant.Backend.Models.Account.Responses
{
    public class AccountRegisterResponse
    {
        /// <summary>
        /// Токен.
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Состояние операции.
        /// </summary>
        public IdentityResult IdentityResult { get; set; }
        
    }
}